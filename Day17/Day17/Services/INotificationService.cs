using Day17.Models;

namespace Day17.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(NotificationModel notification);
        Task StartListeningAsync();
        void StopListening();
        event EventHandler<NotificationModel>? NotificationReceived;
    }
}