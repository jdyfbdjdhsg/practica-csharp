namespace Task1
{
    public class GameResourceManager
    {
        private static GameResourceManager _instance;
        private static readonly object _lock = new object();

        private Dictionary<string, string> _resources;

        private GameResourceManager()
        {
            _resources = new Dictionary<string, string>();
            Console.WriteLine("GameResourceManager создан!");
        }

        public static GameResourceManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameResourceManager();
                    }
                }
            }
            return _instance;
        }

        public void LoadResource(string name)
        {
            if (!_resources.ContainsKey(name))
            {
                _resources[name] = $"Resource:{name}";
                Console.WriteLine($"Ресурс '{name}' загружен");
            }
            else
            {
                Console.WriteLine($"Ресурс '{name}' уже загружен");
            }
        }

        public string GetResource(string name)
        {
            if (_resources.ContainsKey(name))
            {
                return _resources[name];
            }
            Console.WriteLine($"Ресурс '{name}' не найден");
            return null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Singleton: GameResourceManager\n");

            var manager1 = GameResourceManager.GetInstance();
            var manager2 = GameResourceManager.GetInstance();

            Console.WriteLine($"Один и тот же объект? {manager1 == manager2}\n");

            manager1.LoadResource("player_texture");
            manager1.LoadResource("explosion_sound");

            Console.WriteLine($"\nПолучение ресурсов:");
            Console.WriteLine(manager2.GetResource("player_texture"));
            Console.WriteLine(manager2.GetResource("explosion_sound"));
        }
    }
}