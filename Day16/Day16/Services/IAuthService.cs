using Day16.Models;

namespace Day16.Services
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