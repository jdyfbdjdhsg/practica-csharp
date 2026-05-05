using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Day15.Models;

namespace Day15.Services
{
    public class CinemaService : ICinemaService
    {
        private List<Movie> _movies = new();
        private Dictionary<int, List<Seat>> _seatsByMovie = new();
        private List<Booking> _bookings = new();
        private int _nextBookingId = 1;

        public CinemaService()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            _movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Аватар 2", Time = "10:00", Duration = 192, Genre = "Фантастика" },
                new Movie { Id = 2, Title = "Барби", Time = "12:30", Duration = 114, Genre = "Комедия" },
                new Movie { Id = 3, Title = "Оппенгеймер", Time = "15:45", Duration = 180, Genre = "Драма" },
                new Movie { Id = 4, Title = "Джон Уик 4", Time = "19:00", Duration = 169, Genre = "Боевик" },
                new Movie { Id = 5, Title = "Дюна 2", Time = "21:30", Duration = 166, Genre = "Фантастика" }
            };

            string[] rows = { "A", "B", "C", "D", "E" };
            int seatId = 1;

            foreach (var movie in _movies)
            {
                var seats = new List<Seat>();
                foreach (var row in rows)
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        int price = row == "A" ? 5 : (row == "B" ? 4 : 3);
                        seats.Add(new Seat
                        {
                            Id = seatId++,
                            Row = row,
                            Number = i,
                            IsAvailable = true,
                            Price = price,
                            MovieId = movie.Id
                        });
                    }
                }
                _seatsByMovie[movie.Id] = seats;
            }
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            await Task.Delay(1000);
            return _movies;
        }

        public async Task<List<Seat>> GetSeatsAsync(int movieId)
        {
            await Task.Delay(500);

            if (_seatsByMovie.ContainsKey(movieId))
                return _seatsByMovie[movieId].ToList();

            return new List<Seat>();
        }

        public async Task<Booking?> BookSeatAsync(Booking booking)
        {
            await Task.Delay(2000);

            if (!_seatsByMovie.ContainsKey(booking.MovieId))
                return null;

            var seat = _seatsByMovie[booking.MovieId].FirstOrDefault(s => s.SeatNumber == booking.SeatNumber);
            if (seat == null || !seat.IsAvailable)
                return null;

            booking.Id = _nextBookingId++;
            booking.BookingTime = DateTime.Now;
            booking.IsConfirmed = true;

            seat.IsAvailable = false;
            seat.BookingId = booking.Id;

            _bookings.Add(booking);
            return booking;
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            await Task.Delay(1000);

            var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
                return false;

            if (_seatsByMovie.ContainsKey(booking.MovieId))
            {
                var seat = _seatsByMovie[booking.MovieId].FirstOrDefault(s => s.SeatNumber == booking.SeatNumber);
                if (seat != null)
                {
                    seat.IsAvailable = true;
                    seat.BookingId = null;
                }
            }

            _bookings.Remove(booking);
            return true;
        }

        public async Task<List<Booking>> GetBookingsAsync()
        {
            await Task.Delay(500);
            return _bookings.ToList();
        }
    }
}