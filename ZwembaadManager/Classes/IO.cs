using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace ZwembaadManager.Classes
{
    internal static class IO
    {
        private static Stream GetResourceStream(string resourceName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(resourceName);

            var assembly = Assembly.GetExecutingAssembly();
            var manifestNames = assembly.GetManifestResourceNames();
            var stream = TryGetManifestResourceStream(assembly, manifestNames, resourceName) ??
                        TryGetFileSystemStream(resourceName);

            return stream ?? throw new FileNotFoundException(
                $"Resource '{resourceName}' not found. Available resources: {string.Join(", ", manifestNames)}",
                resourceName);
        }

        private static Stream TryGetManifestResourceStream(Assembly assembly, string[] manifestNames, string resourceName)
        {

            var match = manifestNames.FirstOrDefault(n => n.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
            if (match != null)
                return assembly.GetManifestResourceStream(match);

            match = manifestNames.FirstOrDefault(n => n.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));
            if (match != null)
                return assembly.GetManifestResourceStream(match);

            var fileName = Path.GetFileName(resourceName);
            match = manifestNames.FirstOrDefault(n => n.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));
            return match != null ? assembly.GetManifestResourceStream(match) : null;
        }

        private static Stream TryGetFileSystemStream(string resourceName)
        {

            var fullPath = Path.Combine(AppContext.BaseDirectory, resourceName);
            if (File.Exists(fullPath))
                return File.OpenRead(fullPath);

            return File.Exists(resourceName) ? File.OpenRead(resourceName) : null;
        }

        public static T ReadJsonResource<T>(string resourceName)
        {
            using var stream = GetResourceStream(resourceName);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<T>(stream, options)!;
        }

        public static string ReadTextResource(string resourceName)
        {
            using var stream = GetResourceStream(resourceName);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public static List<T> ReadCsvResourceAs<T>(string resourceName, char delimiter = ',') where T : new()
        {
            var text = ReadTextResource(resourceName);
            var lines = text.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return [];

            var headers = ParseCsvLine(lines[0], delimiter).ToArray();
            var properties = GetWritableProperties<T>();

            return lines.Skip(1)
                       .Select(line => CreateObjectFromCsvLine<T>(line, headers, properties, delimiter))
                       .ToList();
        }

        private static PropertyInfo[] GetWritableProperties<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                           .Where(p => p.CanWrite)
                           .ToArray();
        }

        private static T CreateObjectFromCsvLine<T>(string line, string[] headers, PropertyInfo[] properties, char delimiter) where T : new()
        {
            var fields = ParseCsvLine(line, delimiter).ToArray();
            var item = new T();

            for (int i = 0; i < Math.Min(headers.Length, fields.Length); i++)
            {
                SetPropertyValue(item, headers[i], fields[i], properties);
            }

            return item;
        }

        private static void SetPropertyValue<T>(T item, string header, string value, PropertyInfo[] properties)
        {
            if (string.IsNullOrWhiteSpace(header) || string.IsNullOrEmpty(value))
                return;

            var property = properties.FirstOrDefault(p =>
                string.Equals(p.Name, header, StringComparison.OrdinalIgnoreCase));

            if (property == null)
                return;

            try
            {
                var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var converted = Convert.ChangeType(value, targetType);
                property.SetValue(item, converted);
            }
            catch
            {

            }
        }

        public static object ReadResource(string resourceName)
        {
            var extension = Path.GetExtension(resourceName)?.ToLowerInvariant();
            return extension switch
            {
                ".json" => JsonDocument.Parse(ReadTextResource(resourceName)).RootElement.Clone(),
                ".txt" => ReadTextResource(resourceName),
                ".csv" => ReadCsvAsDictionaries(resourceName),
                _ => throw new NotSupportedException($"Extension '{extension}' not supported.")
            };
        }

        private static List<Dictionary<string, string>> ReadCsvAsDictionaries(string resourceName, char delimiter = ',')
        {
            var text = ReadTextResource(resourceName);
            var lines = text.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
                return [];

            var headers = ParseCsvLine(lines[0], delimiter).ToArray();
            return lines.Skip(1)
                       .Select(line => CreateDictionaryFromCsvLine(line, headers, delimiter))
                       .ToList();
        }

        private static Dictionary<string, string> CreateDictionaryFromCsvLine(string line, string[] headers, char delimiter)
        {
            var fields = ParseCsvLine(line, delimiter).ToArray();
            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < headers.Length; i++)
            {
                dictionary[headers[i]] = i < fields.Length ? fields[i] : string.Empty;
            }

            return dictionary;
        }

        private static IEnumerable<string> ParseCsvLine(string line, char delimiter)
        {
            if (line is null) yield break;

            var field = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                var current = line[i];

                if (current == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        field.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (current == delimiter && !inQuotes)
                {
                    yield return field.ToString();
                    field.Clear();
                }
                else
                {
                    field.Append(current);
                }
            }

            yield return field.ToString();
        }
    }
}
