using Microsoft.EntityFrameworkCore;
using Day18.Models;
using System.IO;
using Day18.Helpers;

namespace Day18.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<NotificationModel> Notifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            string dbPath = Path.Combine(dataDirectory, "cinema.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Хешированные пароли
            string adminHash = PasswordHelper.HashPassword("admin123");
            string userHash = PasswordHelper.HashPassword("user123");

            // Начальные данные для пользователей (с хешированными паролями)
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = adminHash, Role = "Admin", FullName = "Администратор", PhoneNumber = "+375 (29) 123-45-67" },
                new User { Id = 2, Username = "user", PasswordHash = userHash, Role = "User", FullName = "Иван Петров", PhoneNumber = "+375 (33) 987-65-43" }
            );

            // Начальные данные для фильмов
            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Title = "Аватар 2", Time = "10:00", Duration = 192, Genre = "Фантастика", Description = "Продолжение эпической истории о мире Пандоры", HallNumber = 1, PosterColor = "#FF1565C0", ShowDate = DateTime.Today },
                new Movie { Id = 2, Title = "Барби", Time = "12:30", Duration = 114, Genre = "Комедия", Description = "Яркая комедия о кукле Барби в реальном мире", HallNumber = 1, PosterColor = "#FFE91E63", ShowDate = DateTime.Today },
                new Movie { Id = 3, Title = "Оппенгеймер", Time = "15:45", Duration = 180, Genre = "Драма", Description = "История создателя атомной бомбы", HallNumber = 2, PosterColor = "#FF424242", ShowDate = DateTime.Today },
                new Movie { Id = 4, Title = "Джон Уик 4", Time = "19:00", Duration = 169, Genre = "Боевик", Description = "Новые приключения легендарного киллера", HallNumber = 2, PosterColor = "#FF7B1FA2", ShowDate = DateTime.Today },
                new Movie { Id = 5, Title = "Дюна 2", Time = "21:30", Duration = 166, Genre = "Фантастика", Description = "Продолжение эпической саги о пустынной планете", HallNumber = 3, PosterColor = "#FFF9A825", ShowDate = DateTime.Today }
            );

            // Начальные данные для сеансов (цены в белорусских рублях)
            modelBuilder.Entity<Session>().HasData(
                new Session { Id = 1, MovieTitle = "Аватар 2", MovieGenre = "Фантастика", Duration = 192, StartTime = DateTime.Today.AddHours(10), HallNumber = 1, TotalSeats = 100, AvailableSeats = 100, TicketPrice = 25 },
                new Session { Id = 2, MovieTitle = "Барби", MovieGenre = "Комедия", Duration = 114, StartTime = DateTime.Today.AddHours(13), HallNumber = 1, TotalSeats = 100, AvailableSeats = 100, TicketPrice = 20 },
                new Session { Id = 3, MovieTitle = "Оппенгеймер", MovieGenre = "Драма", Duration = 180, StartTime = DateTime.Today.AddHours(16), HallNumber = 2, TotalSeats = 120, AvailableSeats = 120, TicketPrice = 28 },
                new Session { Id = 4, MovieTitle = "Джон Уик 4", MovieGenre = "Боевик", Duration = 169, StartTime = DateTime.Today.AddHours(19), HallNumber = 2, TotalSeats = 120, AvailableSeats = 120, TicketPrice = 25 },
                new Session { Id = 5, MovieTitle = "Дюна 2", MovieGenre = "Фантастика", Duration = 166, StartTime = DateTime.Today.AddHours(21), HallNumber = 3, TotalSeats = 80, AvailableSeats = 80, TicketPrice = 30 }
            );

            // Цены для мест (в белорусских рублях)
            // VIP ряд (A, B) - 15 руб., Средний ряд (C, D, E) - 12 руб., Обычный (F, G, H) - 10 руб.
            // Эти цены будут использоваться при создании мест в InitializeSeatsIfNeeded()
        }
    }
}