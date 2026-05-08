using System;

namespace Day18.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string MovieGenre { get; set; } = string.Empty;
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        public int HallNumber { get; set; }
        public int TotalSeats { get; set; } = 100;
        public int AvailableSeats { get; set; } = 100;
        public int TicketPrice { get; set; } = 300;

        public string DisplayText => $"{MovieTitle} - {StartTime:dd.MM HH:mm} - Зал {HallNumber} - {AvailableSeats}/{TotalSeats} мест - {TicketPrice} руб.";
    }
}