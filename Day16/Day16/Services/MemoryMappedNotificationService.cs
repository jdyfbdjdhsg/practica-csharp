using System.IO.MemoryMappedFiles;
using System.Text;
using System.Text.Json;
using Day16.Models;

namespace Day16.Services
{
    public class MemoryMappedNotificationService : INotificationService
    {
        private const string MappedFileName = "CinemaNotifications";
        private const long Capacity = 1024 * 1024;
        private Timer? _timer;

        public event EventHandler<NotificationModel>? NotificationReceived;

        public async Task SendNotificationAsync(NotificationModel notification)
        {
            await Task.Run(() =>
            {
                try
                {
                    using var mmf = MemoryMappedFile.CreateOrOpen(MappedFileName, Capacity);
                    using var stream = mmf.CreateViewStream();
                    var json = JsonSerializer.Serialize(notification);
                    var bytes = Encoding.UTF8.GetBytes(json);
                    stream.Write(bytes, 0, bytes.Length);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"MMF Send Error: {ex.Message}");
                }
            });
        }

        public async Task StartListeningAsync()
        {
            await Task.Run(() =>
            {
                _timer = new Timer(async _ =>
                {
                    try
                    {
                        MemoryMappedFile? mmf = null;
                        try
                        {
                            mmf = MemoryMappedFile.OpenExisting(MappedFileName);
                        }
                        catch
                        {
                            return;
                        }

                        using (mmf)
                        using (var stream = mmf.CreateViewStream())
                        {
                            var buffer = new byte[Capacity];
                            var readCount = stream.Read(buffer, 0, buffer.Length);
                            if (readCount > 0)
                            {
                                var json = Encoding.UTF8.GetString(buffer, 0, readCount);
                                var notification = JsonSerializer.Deserialize<NotificationModel>(json);
                                if (notification != null)
                                {
                                    NotificationReceived?.Invoke(this, notification);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"MMF Receive Error: {ex.Message}");
                    }
                }, null, 0, 2000);
            });
        }

        public void StopListening()
        {
            _timer?.Dispose();
            _timer = null;
        }
    }
}