using Day18.Models;

namespace Day18.Services
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