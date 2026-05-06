using Day16.Helpers;
using Day16.Models;

namespace Day16.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _usersFilePath = "Data/users.json";
        private User? _currentUser;
        private List<User> _users;

        public User? CurrentUser => _currentUser;

        public AuthService()
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
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
                    if (user == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"User not found: {username}");
                        return null;
                    }

                    bool isValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);
                    System.Diagnostics.Debug.WriteLine($"Verification for {username}: {isValid}");

                    if (!isValid)
                        return null;

                    _currentUser = user;
                    return user;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"LoginAsync error: {ex.Message}");
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

                    int maxId = _users.Count > 0 ? _users.Max(u => u.Id) : 0;

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
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"RegisterAsync error: {ex.Message}");
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