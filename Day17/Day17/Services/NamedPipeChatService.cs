using Day17.Models;

namespace Day17.Services
{
    public class NamedPipeChatService : IChatService
    {
        public event EventHandler<ChatMessage>? MessageReceived;

        public async Task SendMessageAsync(ChatMessage message)
        {
            await Task.CompletedTask;
        }

        public async Task StartListeningAsync()
        {
            await Task.CompletedTask;
        }

        public void StopListening()
        {
        }

        public async Task<List<ChatMessage>> GetMessageHistoryAsync()
        {
            return await Task.FromResult(new List<ChatMessage>());
        }
    }
}