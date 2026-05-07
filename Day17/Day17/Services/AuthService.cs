using Day17.Helpers;
using Day17.Models;
using System.IO;

namespace Day17.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _usersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "users.json");
        private User? _currentUser;
        private List<User> _users = new();

        public User? CurrentUser => _currentUser;

        public AuthService()
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                var dir = Path.GetDirectoryName(_usersFilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);

                _users = JsonHelper.ReadFromFile<List<User>>(_usersFilePath) ?? new List<User>();

                if (_users.Count == 0)
                {
                    var adminHash = PasswordHelper.HashPassword("admin123");
                    var userHash = PasswordHelper.HashPassword("user123");

                    _users.Add(new User
                    {
                        Id = 1,
                        Username = "admin",
                        PasswordHash = adminHash,
                        Role = "Admin",
                        FullName = "Администратор",
                        PhoneNumber = "+7 (999) 999-99-99"
                    });
                    _users.Add(new User
                    {
                        Id = 2,
                        Username = "user",
                        PasswordHash = userHash,
                        Role = "User",
                        FullName = "Иван Петров",
                        PhoneNumber = "+7 (888) 888-88-88"
                    });
                    SaveUsers();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadUsers error: {ex.Message}");
                _users = new List<User>();
            }
        }

        private void SaveUsers()
        {
            try
            {
                JsonHelper.WriteToFile(_usersFilePath, _users);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveUsers error: {ex.Message}");
            }
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var user = _users.FirstOrDefault(u => u.Username == username);
                    if (user == null) return null;

                    return !PasswordHelper.VerifyPassword(password, user.PasswordHash) ? null : user;
                }
                catch
                {
                    return null;
                }
            });
        }

        public async Task<bool> RegisterAsync(string username, string password, string fullName, string phoneNumber)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (_users.Any(u => u.Username == username))
                        return false;

                    var maxId = _users.Count > 0 ? _users.Max(u => u.Id) : 0;

                    _users.Add(new User
                    {
                        Id = maxId + 1,
                        Username = username,
                        PasswordHash = PasswordHelper.HashPassword(password),
                        Role = "User",
                        FullName = fullName,
                        PhoneNumber = phoneNumber
                    });

                    SaveUsers();
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        public async Task LogoutAsync()
        {
            await Task.Run(() => _currentUser = null);
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            return await Task.FromResult(_currentUser);
        }
    }
}