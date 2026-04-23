using System;

namespace FactoryWorkers
{
    abstract class Worker
    {
        public string Name { get; set; }

        public Worker(string name)
        {
            Name = name;
        }

        public abstract void DoWork();
    }

    class Welder : Worker
    {
        public Welder(string name) : base(name) { }

        public override void DoWork()
        {
            Console.WriteLine($"{Name} (Сварщик): Свариваю металлические конструкции");
        }
    }

    class Assembler : Worker
    {
        public Assembler(string name) : base(name) { }

        public override void DoWork()
        {
            Console.WriteLine($"{Name} (Сборщик): Собираю детали изделия");
        }
    }

    class Electrician : Worker
    {
        public Electrician(string name) : base(name) { }

        public override void DoWork()
        {
            Console.WriteLine($"{Name} (Электрик): Подключаю электропроводку");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Worker[] workers = new Worker[]
            {
                new Welder("Иван Петров"),
                new Assembler("Сергей Сидоров"),
                new Electrician("Алексей Иванов"),
                new Welder("Дмитрий Козлов"),
                new Assembler("Анна Смирнова")
            };

            Console.WriteLine("Работники завода приступили к работе\n");

            foreach (var worker in workers)
            {
                worker.DoWork();
            }
        }
    }
}