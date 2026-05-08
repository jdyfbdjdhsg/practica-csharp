using Day18.Helpers;
using Day18.Models;
using System.IO;

namespace Day18.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly string _cinemaDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "cinema.json");
        private CinemaData _data = new();

        public CinemaService()
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var dir = Path.GetDirectoryName(_cinemaDataPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);

                _data = JsonHelper.ReadFromFile<CinemaData>(_cinemaDataPath);

                if (_data.Movies.Count == 0)
                {
                    InitializeData();
                    SaveData();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadData error: {ex.Message}");
                InitializeData();
            }
        }

        private void InitializeData()
        {
            _data = new CinemaData();

            _data.Movies = new List<Movie>
            {
                new() { Id = 1, Title = "Аватар 2", Time = "10:00", Duration = 192, Genre = "Фантастика", Description = "Продолжение эпической истории о мире Пандоры", HallNumber = 1, PosterColor = "#FF1565C0" },
                new() { Id = 2, Title = "Барби", Time = "12:30", Duration = 114, Genre = "Комедия", Description = "Яркая комедия о кукле Барби в реальном мире", HallNumber = 1, PosterColor = "#FFE91E63" },
                new() { Id = 3, Title = "Оппенгеймер", Time = "15:45", Duration = 180, Genre = "Драма", Description = "История создателя атомной бомбы", HallNumber = 2, PosterColor = "#FF424242" },
                new() { Id = 4, Title = "Джон Уик 4", Time = "19:00", Duration = 169, Genre = "Боевик", Description = "Новые приключения легендарного киллера", HallNumber = 2, PosterColor = "#FF7B1FA2" },
                new() { Id = 5, Title = "Дюна 2", Time = "21:30", Duration = 166, Genre = "Фантастика", Description = "Продолжение эпической саги о пустынной планете", HallNumber = 3, PosterColor = "#FFF9A825" }
            };

            string[] rows = { "A", "B", "C", "D", "E", "F", "G", "H" };
            int seatId = 1;

            foreach (var movie in _data.Movies)
            {
                for (int rowIdx = 0; rowIdx < rows.Length; rowIdx++)
                {
                    int seatsInRow = rowIdx <= 1 ? 10 : (rowIdx <= 3 ? 12 : 14);
                    for (int i = 1; i <= seatsInRow; i++)
                    {
                        int price = rowIdx <= 1 ? 5 : (rowIdx <= 3 ? 4 : 3);
                        _data.Seats.Add(new Seat
                        {
                            Id = seatId++,
                            Row = rows[rowIdx],
                            Number = i,
                            IsAvailable = true,
                            Price = price,
                            MovieId = movie.Id
                        });
                    }
                }
            }
        }

        private void SaveData()
        {
            try
            {
                JsonHelper.WriteToFile(_cinemaDataPath, _data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveData error: {ex.Message}");
            }
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            return await Task.FromResult(_data.Movies.ToList());
        }

        public async Task<List<Seat>> GetSeatsAsync(int movieId)
        {
            return await Task.FromResult(_data.Seats.Where(s => s.MovieId == movieId).ToList());
        }

        public async Task<Booking?> BookSeatAsync(Booking booking)
        {
            return await Task.Run(() =>
            {
                var seat = _data.Seats.FirstOrDefault(s => s.MovieId == booking.MovieId && s.SeatNumber == booking.SeatNumber);
                if (seat == null || !seat.IsAvailable)
                    return null;

                booking.Id = _data.NextBookingId++;
                booking.BookingTime = DateTime.Now;
                booking.Status = "Confirmed";

                seat.IsAvailable = false;
                seat.BookingId = booking.Id;

                _data.Bookings.Add(booking);
                SaveData();

                return booking;
            });
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            return await Task.Run(() =>
            {
                var booking = _data.Bookings.FirstOrDefault(b => b.Id == bookingId);
                if (booking == null) return false;

                var seat = _data.Seats.FirstOrDefault(s => s.BookingId == bookingId);
                if (seat != null)
                {
                    seat.IsAvailable = true;
                    seat.BookingId = null;
                }

                _data.Bookings.Remove(booking);
                SaveData();

                return true;
            });
        }

        public async Task<List<Booking>> GetBookingsAsync(int? userId = null)
        {
            var bookings = _data.Bookings.ToList();
            if (userId.HasValue)
                bookings = bookings.Where(b => b.UserId == userId.Value).ToList();
            return bookings;
        }
    }
}