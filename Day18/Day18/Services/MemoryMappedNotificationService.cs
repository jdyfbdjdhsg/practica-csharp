using Day18.Models;

namespace Day18.Services
{
    public class MemoryMappedNotificationService : INotificationService
    {
        public event EventHandler<NotificationModel>? NotificationReceived;

        public async Task SendNotificationAsync(NotificationModel notification)
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
    }
}