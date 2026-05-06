namespace Day16.Models
{
    public class CinemaData
    {
        public List<Movie> Movies { get; set; } = new();
        public List<Seat> Seats { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();
        public List<ChatMessage> ChatMessages { get; set; } = new();
        public List<NotificationModel> Notifications { get; set; } = new();
        public int NextBookingId { get; set; } = 1;
        public int NextChatMessageId { get; set; } = 1;
        public int NextNotificationId { get; set; } = 1;
    }
}