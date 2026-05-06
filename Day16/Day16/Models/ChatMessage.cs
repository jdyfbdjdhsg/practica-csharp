namespace Day16.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string SenderRole { get; set; } = "User";
    }
}