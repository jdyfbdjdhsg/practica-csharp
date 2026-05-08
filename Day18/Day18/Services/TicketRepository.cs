using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Day18.Models;

namespace Day18.Services
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DataService _dataService;

        public TicketRepository(DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<Ticket>> GetAllAsync()
        {
            var data = await _dataService.GetDataAsync();
            return data.Tickets.ToList();
        }

        public async Task<List<Ticket>> GetByUserIdAsync(int userId)
        {
            var data = await _dataService.GetDataAsync();
            return data.Tickets.Where(t => t.UserId == userId).ToList();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            var data = await _dataService.GetDataAsync();
            return data.Tickets.FirstOrDefault(t => t.Id == id);
        }

        public async Task AddAsync(Ticket ticket)
        {
            var data = await _dataService.GetDataAsync();
            ticket.Id = data.NextTicketId++;
            ticket.BookingTime = DateTime.Now;
            ticket.Status = "Confirmed";
            data.Tickets.Add(ticket);

            var session = data.Sessions.FirstOrDefault(s => s.Id == ticket.SessionId);
            if (session != null)
            {
                session.AvailableSeats--;
            }

            await _dataService.SaveDataAsync(data);
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            var data = await _dataService.GetDataAsync();
            var index = data.Tickets.FindIndex(t => t.Id == ticket.Id);
            if (index != -1)
            {
                data.Tickets[index] = ticket;
                await _dataService.SaveDataAsync(data);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataService.GetDataAsync();
            var ticket = data.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticket != null)
            {
                var session = data.Sessions.FirstOrDefault(s => s.Id == ticket.SessionId);
                if (session != null)
                {
                    session.AvailableSeats++;
                }

                data.Tickets.Remove(ticket);
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