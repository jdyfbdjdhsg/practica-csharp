using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Day18.Data;
using Day18.Models;

namespace Day18.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly AppDbContext _context;

        public CinemaService()
        {
            _context = new AppDbContext();
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<List<Seat>> GetSeatsAsync(int movieId)
        {
            return await _context.Seats
                .Where(s => s.MovieId == movieId)
                .ToListAsync();
        }

        public async Task<Booking?> BookSeatAsync(Booking booking)
        {
            // Исправлено: используем Row и Number вместо SeatNumber
            var seat = await _context.Seats
                .FirstOrDefaultAsync(s => s.MovieId == booking.MovieId
                    && s.Row == GetRowFromSeatNumber(booking.SeatNumber)
                    && s.Number == GetNumberFromSeatNumber(booking.SeatNumber));

            if (seat == null || !seat.IsAvailable)
                return null;

            booking.BookingTime = DateTime.Now;
            booking.Status = "Confirmed";

            seat.IsAvailable = false;
            seat.BookingId = booking.Id;

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null) return false;

            // Исправлено: используем Row и Number вместо SeatNumber
            var seat = await _context.Seats
                .FirstOrDefaultAsync(s => s.MovieId == booking.MovieId
                    && s.Row == GetRowFromSeatNumber(booking.SeatNumber)
                    && s.Number == GetNumberFromSeatNumber(booking.SeatNumber));

            if (seat != null)
            {
                seat.IsAvailable = true;
                seat.BookingId = null;
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Booking>> GetBookingsAsync(int? userId = null)
        {
            var query = _context.Bookings.AsQueryable();
            if (userId.HasValue)
                query = query.Where(b => b.UserId == userId.Value);
            return await query.ToListAsync();
        }

        // Вспомогательные методы для извлечения Row и Number из SeatNumber (например "A5" -> Row="A", Number=5)
        private string GetRowFromSeatNumber(string seatNumber)
        {
            if (string.IsNullOrEmpty(seatNumber)) return "";
            // Извлекаем буквенную часть (ряд)
            return new string(seatNumber.TakeWhile(c => !char.IsDigit(c)).ToArray());
        }

        private int GetNumberFromSeatNumber(string seatNumber)
        {
            if (string.IsNullOrEmpty(seatNumber)) return 0;
            // Извлекаем числовую часть
            string numberPart = new string(seatNumber.SkipWhile(c => !char.IsDigit(c)).ToArray());
            return int.TryParse(numberPart, out int number) ? number : 0;
        }
    }
}