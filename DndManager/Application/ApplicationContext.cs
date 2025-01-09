using DndManager.Accessor;
using DndManager.Network;
using DndManager.Network.Utility;
using DndManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DndManager.DndApplication
{
    public class ApplicationContext
    {
        private Accessor<ApplicationUser>? _user;

        public ClientSession? ClientSession { get; set; }
        public ServerSession? ServerSession { get; set; }

        public ApplicationContext(IServiceProvider serviceProvider)
        {
            _user = serviceProvider.GetRequiredService<Accessor<ApplicationUser>>();
        }

        public async Task SaveUserAsync()
        {
            if (_user!.Value is PlayerUser)
            {
                await saveObjectToJsonAsync(_user.Value as PlayerUser, "playerUser.json");
            }
            else if (_user.Value is MasterUser)
            {
                await saveObjectToJsonAsync(_user.Value as MasterUser, "masterUser.json");
            }
        }

        public async Task LoadUserAsync(string userType)
        {
            try
            {
                if (userType == "player")
                {
                    var playerPath = Path.Combine(FileSystem.AppDataDirectory, "playerUser.json");
                    if (File.Exists(playerPath))
                    {
                        string playerUserJson = await File.ReadAllTextAsync(playerPath);
                        _user!.Value = JsonSerializer.Deserialize<PlayerUser>(playerUserJson);
                    }
                    else
                    {

                    }
                }
                else if (userType == "master")
                {
                    string masterUserJson = await File.ReadAllTextAsync(Path.Combine(FileSystem.AppDataDirectory, "masterUser.json"));
                    _user!.Value = JsonSerializer.Deserialize<MasterUser>(masterUserJson);
                }
                else
                {
                    throw new ArgumentException("Invalid user type");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the user: {ex.Message}");
            }
        }

        private async Task saveObjectToJsonAsync<T>(T obj, string fileName)
        {
            try
            {
                // Get the path to the app's data directory
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                // Serialize the object to JSON
                string jsonString = JsonSerializer.Serialize(obj);

                // Write the JSON string to the file
                await File.WriteAllTextAsync(filePath, jsonString);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                Console.WriteLine($"An error occurred while saving the object to JSON: {ex.Message}");
            }
        }

        public void SetUser(ApplicationUser user)
        {
            _user.Value = user;
        }

        public ApplicationUser GetUser()
        {
            return _user.Value!;
        }

        public PlayerUser GetPlayerUser()
        {
            return (_user.Value as PlayerUser)!;
        }

        public MasterUser GetMasterUser()
        {
            return (_user.Value as MasterUser)!;
        }

        public bool PlayerUserExists()
        {
            return File.Exists(Path.Combine(FileSystem.AppDataDirectory, "playerUser.json"));
        }

        public bool MasterUserExists()
        {
            return File.Exists(Path.Combine(FileSystem.AppDataDirectory, "masterUser.json"));
        }

        public async Task CreatePlayerUserAsync()
        {
            _user.Value = new PlayerUser();
            await saveObjectToJsonAsync(_user, "playerUser.json");
        }

        public async Task CreateMasterUserAsync()
        {
            _user.Value = new MasterUser();
            await saveObjectToJsonAsync(_user, "masterUser.json");
        }

        public async Task CreateClientSessionAsync()
        {
            //ClientSession = new ClientSession();
        }
    }
}
