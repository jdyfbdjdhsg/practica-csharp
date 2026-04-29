namespace Task2
{
    public interface ICacheStrategy
    {
        void Store(string key, string value);
        string Retrieve(string key);
    }

    public class InMemoryCache : ICacheStrategy
    {
        private Dictionary<string, string> _cache = new Dictionary<string, string>();

        public void Store(string key, string value)
        {
            _cache[key] = value;
            Console.WriteLine($"[InMemoryCache] Сохранено: {key} = {value}");
        }

        public string Retrieve(string key)
        {
            return _cache.ContainsKey(key) ? _cache[key] : null;
        }
    }

    public class DistributedCache : ICacheStrategy
    {
        public void Store(string key, string value)
        {
            Console.WriteLine($"[DistributedCache] Сохранено в Redis: {key} = {value}");
        }

        public string Retrieve(string key)
        {
            return $"Distributed data for {key}";
        }
    }

    public class NoCache : ICacheStrategy
    {
        public void Store(string key, string value)
        {
            Console.WriteLine($"[NoCache] Кеширование отключено: {key} не сохранено");
        }

        public string Retrieve(string key)
        {
            Console.WriteLine($"[NoCache] Кеширование отключено, данных нет");
            return null;
        }
    }

    public class CacheManager
    {
        private ICacheStrategy _strategy;

        public void SetStrategy(ICacheStrategy strategy)
        {
            _strategy = strategy;
            Console.WriteLine($"\nСтратегия кеширования изменена на {strategy.GetType().Name}");
        }

        public void CacheData(string key, string value)
        {
            _strategy.Store(key, value);
        }

        public string GetData(string key)
        {
            return _strategy.Retrieve(key);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Strategy Pattern\n");

            var cacheManager = new CacheManager();

            cacheManager.SetStrategy(new InMemoryCache());
            cacheManager.CacheData("user:1", "John");
            Console.WriteLine($"Получено: {cacheManager.GetData("user:1")}");

            cacheManager.SetStrategy(new DistributedCache());
            cacheManager.CacheData("user:2", "Jane");
            Console.WriteLine($"Получено: {cacheManager.GetData("user:2")}");

            cacheManager.SetStrategy(new NoCache());
            cacheManager.CacheData("user:3", "Bob");
            Console.WriteLine($"Получено: {cacheManager.GetData("user:3")}");

            Console.WriteLine("\nBuilder Pattern\n");
        }
    }
}
