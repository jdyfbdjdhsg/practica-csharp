using Day18.Data;
using Day18.Models;

namespace Day18.Services
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        public SessionRepository(AppDbContext context) : base(context)
        {
        }
    }

    public interface ISessionRepository : IRepository<Session>
    {
    }
}