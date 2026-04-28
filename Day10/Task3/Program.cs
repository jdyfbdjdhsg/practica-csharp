namespace Task3
{
    public interface ISystemObserver
    {
        void Update(string message);
    }

    public class ServerMonitor
    {
        private List<ISystemObserver> _observers = new List<ISystemObserver>();
        private int _serverLoad;
        private readonly int _criticalLoad = 80;

        public void Subscribe(ISystemObserver observer)
        {
            _observers.Add(observer);
            Console.WriteLine($"{observer.GetType().Name} подписался на уведомления");
        }

        public void Unsubscribe(ISystemObserver observer)
        {
            _observers.Remove(observer);
            Console.WriteLine($"{observer.GetType().Name} отписался");
        }

        public void CheckServerLoad()
        {
            Random rand = new Random();
            _serverLoad = rand.Next(50, 101);

            Console.WriteLine($"\nТекущая нагрузка сервера: {_serverLoad}%");

            if (_serverLoad > _criticalLoad)
            {
                NotifyObservers($"КРИТИЧЕСКАЯ НАГРУЗКА: {_serverLoad}%! Сервер перегружен!");
            }
        }

        private void NotifyObservers(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }

    public class Admin : ISystemObserver
    {
        private string _name;

        public Admin(string name)
        {
            _name = name;
        }

        public void Update(string message)
        {
            Console.WriteLine($"📧 Администратор {_name} получил уведомление: {message}");
            Console.WriteLine($"   Действие: Админ {_name} запускает диагностику...\n");
        }
    }

    public class DevOps : ISystemObserver
    {
        private string _team;

        public DevOps(string team)
        {
            _team = team;
        }

        public void Update(string message)
        {
            Console.WriteLine($"💻 DevOps ({_team}) получил уведомление: {message}");
            Console.WriteLine($"   Действие: Команда {_team} масштабирует серверы...\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Observer: Мониторинг серверов ===\n");

            var monitor = new ServerMonitor();

            var alice = new Admin("Alice");
            var bob = new Admin("Bob");
            var devopsTeam = new DevOps("Backend Team");

            monitor.Subscribe(alice);
            monitor.Subscribe(bob);
            monitor.Subscribe(devopsTeam);

            Console.WriteLine("\nНачинаем мониторинг сервера...\n");

            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"--- Проверка #{i} ---");
                monitor.CheckServerLoad();
                Thread.Sleep(1000);
            }
        }
    }
}