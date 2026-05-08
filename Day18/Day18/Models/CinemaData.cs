using System.Collections.Generic;

namespace Day18.Models
{
    public class CinemaData
    {
        // Существующие поля
        public List<Movie> Movies { get; set; } = new();
        public List<Seat> Seats { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();
        public List<ChatMessage> ChatMessages { get; set; } = new();
        public List<NotificationModel> Notifications { get; set; } = new();

        // Для варианта 9
        public List<Session> Sessions { get; set; } = new();
        public List<Ticket> Tickets { get; set; } = new();

        // Счетчики ID
        public int NextBookingId { get; set; } = 1;
        public int NextChatMessageId { get; set; } = 1;
        public int NextNotificationId { get; set; } = 1;
        public int NextSessionId { get; set; } = 6;
        public int NextTicketId { get; set; } = 1;
    }
}