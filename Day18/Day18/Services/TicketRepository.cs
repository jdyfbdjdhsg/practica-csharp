using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Day18.Data;
using Day18.Models;

namespace Day18.Services
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Ticket>> GetByUserIdAsync(int userId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
    }

    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<List<Ticket>> GetByUserIdAsync(int userId);
    }
}