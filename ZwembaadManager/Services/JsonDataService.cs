using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZwembaadManager.Classes;
using ZwembaadManager.Models;
using System.Reflection;
using System.Linq;

namespace ZwembaadManager.Services
{
    public class JsonDataService
    {
        private readonly string _usersFilePath;
        private readonly string _clubsFilePath;
        private readonly string _meetsFilePath;
        
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
            _clubsFilePath = Path.Combine(resourcesPath, "Clubs.json");
            _meetsFilePath = Path.Combine(resourcesPath, "Meets.json");
            Directory.CreateDirectory(resourcesPath);
            
            InitializeUsersFileIfNeeded();
            InitializeClubsFileIfNeeded();
            InitializeMeetsFileIfNeeded();
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

        private void InitializeClubsFileIfNeeded()
        {
            if (!File.Exists(_clubsFilePath))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "ZwembaadManager.Resources.Clubs.json";
                
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    string content = reader.ReadToEnd();
                    File.WriteAllText(_clubsFilePath, content);
                }
                else
                {
                    File.WriteAllText(_clubsFilePath, "[]");
                }
            }
        }

        private void InitializeMeetsFileIfNeeded()
        {
            if (!File.Exists(_meetsFilePath))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "ZwembaadManager.Resources.Meets.json";
                
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    using var reader = new StreamReader(stream);
                    string content = reader.ReadToEnd();
                    File.WriteAllText(_meetsFilePath, content);
                }
                else
                {
                    File.WriteAllText(_meetsFilePath, "[]");
                }
            }
        }

        // User-related methods
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

        // Club-related methods
        public async Task<List<Club>> LoadClubsAsync()
        {
            try
            {
                if (!File.Exists(_clubsFilePath))
                {
                    return new List<Club>();
                }

                string jsonContent = await File.ReadAllTextAsync(_clubsFilePath);
                var clubs = JsonSerializer.Deserialize<List<Club>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return clubs ?? new List<Club>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load clubs: {ex.Message}", ex);
            }
        }

        public async Task SaveClubsAsync(List<Club> clubs)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(clubs, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                await File.WriteAllTextAsync(_clubsFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save clubs: {ex.Message}", ex);
            }
        }

        public async Task<int> GetNextClubIdAsync()
        {
            var clubs = await LoadClubsAsync();
            return clubs.Count > 0 ? clubs.Max(c => c.Id) + 1 : 1;
        }

        public async Task AddClubAsync(Club newClub)
        {
            var clubs = await LoadClubsAsync();
            
            // Check for duplicates
            var existingClub = clubs.Find(c => c.Name.Equals(newClub.Name, StringComparison.OrdinalIgnoreCase) ||
                                              c.Abbreviation.Equals(newClub.Abbreviation, StringComparison.OrdinalIgnoreCase));
            
            if (existingClub != null)
            {
                throw new InvalidOperationException($"A club with the name '{newClub.Name}' or abbreviation '{newClub.Abbreviation}' already exists.");
            }

            newClub.Id = await GetNextClubIdAsync();
            newClub.CreatedDate = DateTime.Now;
            newClub.ModifiedDate = DateTime.Now;
            
            clubs.Add(newClub);
            await SaveClubsAsync(clubs);
        }

        // Meet-related methods
        public async Task<List<Meet>> LoadMeetsAsync()
        {
            try
            {
                if (!File.Exists(_meetsFilePath))
                {
                    return new List<Meet>();
                }

                string jsonContent = await File.ReadAllTextAsync(_meetsFilePath);
                var meets = JsonSerializer.Deserialize<List<Meet>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return meets ?? new List<Meet>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load meets: {ex.Message}", ex);
            }
        }

        public async Task SaveMeetsAsync(List<Meet> meets)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(meets, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                await File.WriteAllTextAsync(_meetsFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save meets: {ex.Message}", ex);
            }
        }

        public async Task<int> GetNextMeetIdAsync()
        {
            var meets = await LoadMeetsAsync();
            return meets.Count > 0 ? meets.Max(m => m.Id) + 1 : 1;
        }

        public async Task AddMeetAsync(Meet newMeet)
        {
            var meets = await LoadMeetsAsync();
            
            // Check for duplicates (same name on same date)
            var existingMeet = meets.Find(m => m.Name.Equals(newMeet.Name, StringComparison.OrdinalIgnoreCase) &&
                                              m.Date.Date == newMeet.Date.Date);
            
            if (existingMeet != null)
            {
                throw new InvalidOperationException($"A meet with the name '{newMeet.Name}' on {newMeet.Date:yyyy-MM-dd} already exists.");
            }

            newMeet.Id = await GetNextMeetIdAsync();
            newMeet.CreatedDate = DateTime.Now;
            newMeet.ModifiedDate = DateTime.Now;
            
            meets.Add(newMeet);
            await SaveMeetsAsync(meets);
        }

        // File path getters
        public string GetUsersFilePath() => _usersFilePath;
        public string GetClubsFilePath() => _clubsFilePath;
        public string GetMeetsFilePath() => _meetsFilePath;
    }
}