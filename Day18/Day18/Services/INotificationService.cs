using Day18.Models;

namespace Day18.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(NotificationModel notification);
        Task StartListeningAsync();
        void StopListening();
        event EventHandler<NotificationModel>? NotificationReceived;
    }
}