using Day16.Models;

namespace Day16.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(NotificationModel notification);
        Task StartListeningAsync();
        void StopListening();
        event EventHandler<NotificationModel>? NotificationReceived;
    }
}