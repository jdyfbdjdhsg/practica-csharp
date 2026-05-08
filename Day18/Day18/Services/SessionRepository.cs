using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Day18.Models;

namespace Day18.Services
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DataService _dataService;

        public SessionRepository(DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<Session>> GetAllAsync()
        {
            var data = await _dataService.GetDataAsync();
            return data.Sessions.ToList();
        }

        public async Task<Session?> GetByIdAsync(int id)
        {
            var data = await _dataService.GetDataAsync();
            return data.Sessions.FirstOrDefault(s => s.Id == id);
        }

        public async Task AddAsync(Session session)
        {
            var data = await _dataService.GetDataAsync();
            session.Id = data.NextSessionId++;
            data.Sessions.Add(session);
            await _dataService.SaveDataAsync(data);
        }

        public async Task UpdateAsync(Session session)
        {
            var data = await _dataService.GetDataAsync();
            var index = data.Sessions.FindIndex(s => s.Id == session.Id);
            if (index != -1)
            {
                data.Sessions[index] = session;
                await _dataService.SaveDataAsync(data);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataService.GetDataAsync();
            var session = data.Sessions.FirstOrDefault(s => s.Id == id);
            if (session != null)
            {
                data.Sessions.Remove(session);
                await _dataService.SaveDataAsync(data);
            }
        }

        public async Task SaveChangesAsync()
        {
            var data = await _dataService.GetDataAsync();
            await _dataService.SaveDataAsync(data);
        }
    }
}