using Day16.Models;

namespace Day16.Services
{
    public interface IChatService
    {
        Task SendMessageAsync(ChatMessage message);
        Task StartListeningAsync();
        void StopListening();
        Task<List<ChatMessage>> GetMessageHistoryAsync();
        event EventHandler<ChatMessage>? MessageReceived;
    }
}