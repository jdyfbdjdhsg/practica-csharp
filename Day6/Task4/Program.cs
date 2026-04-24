using System;

public class UserLoginEventArgs : EventArgs
{
    public string Username { get; set; }
    public DateTime LoginTime { get; set; }
}

public class UserLoginManager
{
    public event EventHandler<UserLoginEventArgs> UserLoggedIn;

    public void Login(string username)
    {
        Console.WriteLine($"Пользователь {username} вошел в систему");

        UserLoggedIn?.Invoke(this, new UserLoginEventArgs
        {
            Username = username,
            LoginTime = DateTime.Now
        });
    }
}

public class SecuritySystem
{
    public void CheckAccess(object sender, UserLoginEventArgs e)
    {
        Console.WriteLine($"Проверка доступа для: {e.Username}");
    }
}

public class NotificationService
{
    public void SendNotification(object sender, UserLoginEventArgs e)
    {
        Console.WriteLine($"Уведомление: {e.Username} вошел в {e.LoginTime}");
    }
}

public class LoginObserver
{
    private UserLoginManager _manager;
    private SecuritySystem _security;
    private NotificationService _notification;

    public LoginObserver(UserLoginManager manager)
    {
        _manager = manager;
        _security = new SecuritySystem();
        _notification = new NotificationService();

        SubscribeAll();
    }

    private void SubscribeAll()
    {
        _manager.UserLoggedIn += _security.CheckAccess;
        _manager.UserLoggedIn += _notification.SendNotification;
        Console.WriteLine("Все подписчики зарегистрированы\n");
    }

    public void UnsubscribeAll()
    {
        _manager.UserLoggedIn -= _security.CheckAccess;
        _manager.UserLoggedIn -= _notification.SendNotification;
    }
}

class Program
{
    static void Main()
    {
        UserLoginManager manager = new UserLoginManager();

        LoginObserver observer = new LoginObserver(manager);

        manager.Login("Alice");
        manager.Login("Bob");
        manager.Login("Charlie");
    }
}