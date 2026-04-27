namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            var writer = new CustomerFileWriter();

            var customers = new List<Customer>
            {
                new Customer(1, "Иван Петров"),
                new Customer(2, "Мария Сидорова"),
                new Customer(1, "Иван Петров"), 
                new Customer(3, "Алексей Иванов"),
                new Customer(2, "Мария Сидорова") 
            };

            Console.WriteLine("Запись уникальных клиентов...");
            writer.WriteUniqueCustomers(customers);
            Console.WriteLine("Готово!");

            string content = File.ReadAllText("file.data");
            Console.WriteLine("\nСодержимое file.data:");
            Console.WriteLine(content);
        }
    }
}