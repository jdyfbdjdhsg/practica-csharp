namespace Task3
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
            writer.WriteUniqueCustomers(customers);

            var reader = new CustomerFileReader();
            var processor = new CustomerProcessor();

            var loadedCustomers = reader.ReadCustomers();
            Console.WriteLine($"Загружено клиентов: {loadedCustomers.Count}");

            var duplicates = processor.FindDuplicates(loadedCustomers);
            Console.WriteLine($"\nНайдено дубликатов по Id: {duplicates.Count}");

            foreach (var dup in duplicates)
            {
                Console.WriteLine($"Дубликат Id={dup.Id}: {string.Join(", ", dup.Names)}");
            }
        }
    }
}