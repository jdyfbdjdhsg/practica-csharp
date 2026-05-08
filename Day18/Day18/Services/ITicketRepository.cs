using System.Collections.Generic;
using System.Threading.Tasks;
using Day18.Models;

namespace Day18.Services
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetAllAsync();
        Task<List<Ticket>> GetByUserIdAsync(int userId);
        Task<Ticket?> GetByIdAsync(int id);
        Task AddAsync(Ticket ticket);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}