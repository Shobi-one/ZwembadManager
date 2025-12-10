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
        public static string ReadTextResource(string resourceName)
        {
            using var stream = GetResourceStream(resourceName);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public static T ReadJsonResource<T>(string resourceName)
        {
            using var stream = GetResourceStream(resourceName);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<T>(stream, options)!;
        }


        public static object ReadResource(string resourceName)
        {
            var extension = Path.GetExtension(resourceName)?.ToLowerInvariant();
            return extension switch
            {
                ".json" => JsonDocument.Parse(ReadTextResource(resourceName)).RootElement.Clone(),
                ".txt" => ReadTextResource(resourceName),
                _ => throw new NotSupportedException($"Extension '{extension}' not supported.")
            };
        }
    }
}
