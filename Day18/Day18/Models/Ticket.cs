using System.ComponentModel.DataAnnotations;

namespace Day18.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;
        public int Price { get; set; }
        public DateTime BookingTime { get; set; }
        public string Status { get; set; } = "Confirmed";
        public string MovieTitle { get; set; } = string.Empty;
        public DateTime SessionTime { get; set; }

        public string DisplayText => $"{MovieTitle} - {SessionTime:dd.MM HH:mm} - Место {SeatNumber} - {Price} руб.";
    }
}