using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZwembaadManager.Classes;
using System.Reflection;
using System.Linq;

namespace ZwembaadManager.Services
{
    public class JsonDataService
    {
        private readonly string _usersFilePath;
        
        public JsonDataService()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;
            string resourcesPath;
            
#if DEBUG
            string projectRoot = Path.GetFullPath(Path.Combine(assemblyDirectory, "..", "..", ".."));
            resourcesPath = Path.Combine(projectRoot, "Resources");
#else
            // In Release mode, look for Resources folder next to the executable
            resourcesPath = Path.Combine(assemblyDirectory, "Resources");
            
            // If it doesn't exist, create it and copy the embedded resource
            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
            }
#endif
            
            _usersFilePath = Path.Combine(resourcesPath, "Users.json");
            Directory.CreateDirectory(resourcesPath);
            
            InitializeUsersFileIfNeeded();
        }

        private void InitializeUsersFileIfNeeded()
        {
            if (!File.Exists(_usersFilePath))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "ZwembaadManager.Resources.Users.json";
                
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    string content = reader.ReadToEnd();
                    File.WriteAllText(_usersFilePath, content);
                }
                else
                {
                    File.WriteAllText(_usersFilePath, "[]");
                }
            }
        }

        public async Task<List<User>> LoadUsersAsync()
        {
            try
            {
                if (!File.Exists(_usersFilePath))
                {
                    return new List<User>();
                }

                string jsonContent = await File.ReadAllTextAsync(_usersFilePath);
                var users = JsonSerializer.Deserialize<List<User>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return users ?? new List<User>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load users: {ex.Message}", ex);
            }
        }

        public async Task SaveUsersAsync(List<User> users)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(users, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                await File.WriteAllTextAsync(_usersFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save users: {ex.Message}", ex);
            }
        }

        public async Task<int> GetNextUserIdAsync()
        {
            var users = await LoadUsersAsync();
            return users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
        }

        public async Task AddUserAsync(User newUser)
        {
            var users = await LoadUsersAsync();
            newUser.Id = await GetNextUserIdAsync();
            
            users.Add(newUser);
            await SaveUsersAsync(users);
        }

        public string GetUsersFilePath() => _usersFilePath;
    }
}