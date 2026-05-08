using Day18.Models;

namespace Day18.Services
{
    public interface ICinemaService
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<List<Seat>> GetSeatsAsync(int movieId);
        Task<Booking?> BookSeatAsync(Booking booking);
        Task<bool> CancelBookingAsync(int bookingId);
        Task<List<Booking>> GetBookingsAsync(int? userId = null);
    }
}