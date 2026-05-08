using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Day18.Helpers;
using Day18.Models;

namespace Day18.Services
{
    public class DataService
    {
        private readonly string _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "cinema.json");
        private CinemaData _data;

        public DataService()
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var dir = Path.GetDirectoryName(_dataPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);

                _data = JsonHelper.ReadFromFile<CinemaData>(_dataPath);

                // ОТЛАДКА
                System.Diagnostics.Debug.WriteLine($"=== LoadData ===");
                System.Diagnostics.Debug.WriteLine($"Sessions count: {_data.Sessions?.Count ?? 0}");
                System.Diagnostics.Debug.WriteLine($"Movies count: {_data.Movies?.Count ?? 0}");

                if (_data.Sessions == null || _data.Sessions.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Инициализируем данные...");
                    InitializeData();
                    SaveDataAsync(_data).Wait();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadData Error: {ex.Message}");
                InitializeData();
            }
        }

        private void InitializeData()
        {
            _data = new CinemaData();

            // Фильмы
            _data.Movies = new List<Movie>
            {
                new() { Id = 1, Title = "Аватар 2", Time = "10:00", Duration = 192, Genre = "Фантастика", Description = "Продолжение эпической истории о мире Пандоры", HallNumber = 1, PosterColor = "#FF1565C0" },
                new() { Id = 2, Title = "Барби", Time = "12:30", Duration = 114, Genre = "Комедия", Description = "Яркая комедия о кукле Барби в реальном мире", HallNumber = 1, PosterColor = "#FFE91E63" },
                new() { Id = 3, Title = "Оппенгеймер", Time = "15:45", Duration = 180, Genre = "Драма", Description = "История создателя атомной бомбы", HallNumber = 2, PosterColor = "#FF424242" },
                new() { Id = 4, Title = "Джон Уик 4", Time = "19:00", Duration = 169, Genre = "Боевик", Description = "Новые приключения легендарного киллера", HallNumber = 2, PosterColor = "#FF7B1FA2" },
                new() { Id = 5, Title = "Дюна 2", Time = "21:30", Duration = 166, Genre = "Фантастика", Description = "Продолжение эпической саги о пустынной планете", HallNumber = 3, PosterColor = "#FFF9A825" }
            };

            // Места
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

            // СЕАНСЫ ДЛЯ ВАРИАНТА 9
            _data.Sessions = new List<Session>
            {
                new() { Id = 1, MovieTitle = "Аватар 2", MovieGenre = "Фантастика", Duration = 192, StartTime = DateTime.Today.AddHours(10), HallNumber = 1, TotalSeats = 100, AvailableSeats = 100, TicketPrice = 400 },
                new() { Id = 2, MovieTitle = "Барби", MovieGenre = "Комедия", Duration = 114, StartTime = DateTime.Today.AddHours(13), HallNumber = 1, TotalSeats = 100, AvailableSeats = 100, TicketPrice = 350 },
                new() { Id = 3, MovieTitle = "Оппенгеймер", MovieGenre = "Драма", Duration = 180, StartTime = DateTime.Today.AddHours(16), HallNumber = 2, TotalSeats = 120, AvailableSeats = 120, TicketPrice = 450 },
                new() { Id = 4, MovieTitle = "Джон Уик 4", MovieGenre = "Боевик", Duration = 169, StartTime = DateTime.Today.AddHours(19), HallNumber = 2, TotalSeats = 120, AvailableSeats = 120, TicketPrice = 400 },
                new() { Id = 5, MovieTitle = "Дюна 2", MovieGenre = "Фантастика", Duration = 166, StartTime = DateTime.Today.AddHours(21), HallNumber = 3, TotalSeats = 80, AvailableSeats = 80, TicketPrice = 500 }
            };

            // ОТЛАДКА
            System.Diagnostics.Debug.WriteLine($"=== InitializeData ===");
            System.Diagnostics.Debug.WriteLine($"Создано сеансов: {_data.Sessions.Count}");
            foreach (var s in _data.Sessions)
            {
                System.Diagnostics.Debug.WriteLine($"  - {s.MovieTitle} ({s.StartTime})");
            }

            _data.NextSessionId = 6;
            _data.NextTicketId = 1;
            _data.NextBookingId = 1;
            _data.NextChatMessageId = 1;
            _data.NextNotificationId = 1;
        }

        public async Task<CinemaData> GetDataAsync()
        {
            return await Task.FromResult(_data);
        }

        public async Task SaveDataAsync(CinemaData data)
        {
            _data = data;
            await Task.Run(() => JsonHelper.WriteToFile(_dataPath, _data));
        }
    }
}