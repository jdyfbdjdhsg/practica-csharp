
namespace Task3
{
    public class CustomerFileWriter
    {
        public void WriteUniqueCustomers(List<Customer> customers)
        {
            var uniqueCustomers = customers.Distinct().ToList();
            using (StreamWriter writer = new StreamWriter("file.data"))
            {
                foreach (var customer in uniqueCustomers)
                {
                    writer.WriteLine(customer.ToString());
                }
            }
        }
    }
}
