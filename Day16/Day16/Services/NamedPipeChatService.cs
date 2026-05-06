using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using Day16.Models;

namespace Day16.Services
{
    public class NamedPipeChatService : IChatService
    {
        private const string PipeName = "CinemaChatPipe";
        private List<ChatMessage> _messageHistory = new();
        private bool _isListening = true;

        public event EventHandler<ChatMessage>? MessageReceived;

        public async Task SendMessageAsync(ChatMessage message)
        {
            await Task.Run(async () =>
            {
                try
                {
                    using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
                    await client.ConnectAsync(1000);

                    var json = JsonSerializer.Serialize(message);
                    var bytes = Encoding.UTF8.GetBytes(json);
                    await client.WriteAsync(bytes, 0, bytes.Length);
                    await client.FlushAsync();

                    _messageHistory.Add(message);
                    MessageReceived?.Invoke(this, message);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Pipe Send Error: {ex.Message}");
                }
            });
        }

        public async Task StartListeningAsync()
        {
            await Task.Run(async () =>
            {
                while (_isListening)
                {
                    try
                    {
                        using var serverStream = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 10);
                        await serverStream.WaitForConnectionAsync();

                        var buffer = new byte[4096];
                        var readCount = await serverStream.ReadAsync(buffer, 0, buffer.Length);

                        if (readCount > 0)
                        {
                            var json = Encoding.UTF8.GetString(buffer, 0, readCount);
                            var message = JsonSerializer.Deserialize<ChatMessage>(json);
                            if (message != null)
                            {
                                _messageHistory.Add(message);
                                MessageReceived?.Invoke(this, message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Pipe Listen Error: {ex.Message}");
                        await Task.Delay(1000);
                    }
                }
            });
        }

        public void StopListening()
        {
            _isListening = false;
        }

        public async Task<List<ChatMessage>> GetMessageHistoryAsync()
        {
            return await Task.FromResult(_messageHistory.ToList());
        }
    }
}