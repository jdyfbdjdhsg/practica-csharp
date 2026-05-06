namespace Day16.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int Price { get; set; }
        public int? BookingId { get; set; }
        public int MovieId { get; set; }

        public string SeatNumber => $"{Row}{Number}";
    }
}