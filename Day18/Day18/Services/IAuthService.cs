using Day18.Models;

namespace Day18.Services
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password, string fullName, string phoneNumber);
        Task<User?> GetCurrentUserAsync();
        Task LogoutAsync();
        User? CurrentUser { get; }
    }
}