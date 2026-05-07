using Day17.Models;

namespace Day17.Services
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