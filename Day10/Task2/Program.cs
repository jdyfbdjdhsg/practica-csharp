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

    public class Computer
    {
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string Storage { get; set; }
        public string GraphicsCard { get; set; }

        public void ShowSpecs()
        {
            Console.WriteLine($"Компьютер: CPU={CPU}, RAM={RAM}, Storage={Storage}, GPU={GraphicsCard}");
        }
    }

    public interface IComputerBuilder
    {
        IComputerBuilder SetCPU(string cpu);
        IComputerBuilder SetRAM(string ram);
        IComputerBuilder SetStorage(string storage);
        IComputerBuilder SetGraphicsCard(string gpu);
        Computer Build();
    }

    public class GamingComputerBuilder : IComputerBuilder
    {
        private Computer _computer = new Computer();

        public IComputerBuilder SetCPU(string cpu)
        {
            _computer.CPU = cpu;
            return this;
        }

        public IComputerBuilder SetRAM(string ram)
        {
            _computer.RAM = ram;
            return this;
        }

        public IComputerBuilder SetStorage(string storage)
        {
            _computer.Storage = storage;
            return this;
        }

        public IComputerBuilder SetGraphicsCard(string gpu)
        {
            _computer.GraphicsCard = gpu;
            return this;
        }

        public Computer Build()
        {
            return _computer;
        }
    }

    public class ComputerDirector
    {
        public Computer BuildGamingPC()
        {
            return new GamingComputerBuilder()
                .SetCPU("Intel i9")
                .SetRAM("32GB DDR5")
                .SetStorage("1TB NVMe SSD")
                .SetGraphicsCard("NVIDIA RTX 4080")
                .Build();
        }

        public Computer BuildOfficePC()
        {
            return new GamingComputerBuilder()
                .SetCPU("Intel i5")
                .SetRAM("16GB DDR4")
                .SetStorage("512GB SSD")
                .SetGraphicsCard("Integrated")
                .Build();
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

            var builder = new GamingComputerBuilder();
            var customPC = builder
                .SetCPU("AMD Ryzen 7")
                .SetRAM("64GB DDR5")
                .SetStorage("2TB SSD")
                .SetGraphicsCard("AMD RX 7900")
                .Build();

            Console.Write("Пользовательский: ");
            customPC.ShowSpecs();

            var director = new ComputerDirector();
            var gamingPC = director.BuildGamingPC();
            var officePC = director.BuildOfficePC();

            Console.Write("Игровой: ");
            gamingPC.ShowSpecs();
            Console.Write("Офисный: ");
            officePC.ShowSpecs();
        }
    }
}