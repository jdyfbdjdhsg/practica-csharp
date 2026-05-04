namespace Day13
{
    public class Booking
    {
        public string Customer { get; set; } = string.Empty;
        public string Movie { get; set; } = string.Empty;
        public string Seat { get; set; } = string.Empty;

        public string DisplayText => $"{Customer} — {Movie} (Место: {Seat})";
    }
}