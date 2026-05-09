using System.ComponentModel.DataAnnotations;

namespace Day18.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;
        public int Price { get; set; }
        public DateTime BookingTime { get; set; }
        public string Status { get; set; } = "Confirmed";

        public string DisplayText => $"{CustomerName} - {MovieTitle} ({SeatNumber}) - {Price} руб.";
    }
}