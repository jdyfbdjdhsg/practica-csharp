using System.Collections.Generic;
using System.Threading.Tasks;
using Day15.Models;

namespace Day15.Services
{
    public interface ICinemaService
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<List<Seat>> GetSeatsAsync(int movieId);
        Task<Booking?> BookSeatAsync(Booking booking);
        Task<bool> CancelBookingAsync(int bookingId);
        Task<List<Booking>> GetBookingsAsync();
    }
}