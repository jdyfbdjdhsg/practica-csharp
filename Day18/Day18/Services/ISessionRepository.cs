using System.Collections.Generic;
using System.Threading.Tasks;
using Day18.Models;

namespace Day18.Services
{
    public interface ISessionRepository
    {
        Task<List<Session>> GetAllAsync();
        Task<Session?> GetByIdAsync(int id);
        Task AddAsync(Session session);
        Task UpdateAsync(Session session);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}