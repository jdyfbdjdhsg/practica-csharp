namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Фабричный метод\n");

            AnimalFactory[] factories = new AnimalFactory[]
            {
                new DogFactory(),
                new CatFactory(),
                new BirdFactory()
            };

            foreach (var factory in factories)
            {
                IAnimal animal = factory.CreateAnimal();
                Console.WriteLine($"{animal.GetType().Name}: {animal.MakeSound()}");
            }
        }
    }
}