using System;

namespace Day15.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;
        public int Price { get; set; }
        public DateTime BookingTime { get; set; }
        public bool IsConfirmed { get; set; }

        public string DisplayText => $"{CustomerName} - {MovieTitle} ({SeatNumber}) - {Price} руб.";
    }
}