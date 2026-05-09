using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Day18.Data;
using Day18.Helpers;
using Day18.Models;
using System.Linq;

namespace Day18.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private User? _currentUser;

        public AuthService()
        {
            _context = new AppDbContext();
        }

        public User? CurrentUser => _currentUser;

        public async Task<User?> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null) return null;

                if (password == user.PasswordHash || PasswordHelper.VerifyPassword(password, user.PasswordHash))
                {
                    _currentUser = user;
                    return user;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> RegisterAsync(string username, string password, string fullName, string phoneNumber)
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (existingUser != null)
                    return false;

                var maxId = await _context.Users.AnyAsync() ? await _context.Users.MaxAsync(u => u.Id) : 0;

                var newUser = new User
                {
                    Id = maxId + 1,
                    Username = username,
                    PasswordHash = PasswordHelper.HashPassword(password),
                    Role = "User",
                    FullName = fullName,
                    PhoneNumber = phoneNumber
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            _currentUser = null;
            await Task.CompletedTask;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            return await Task.FromResult(_currentUser);
        }
    }
}