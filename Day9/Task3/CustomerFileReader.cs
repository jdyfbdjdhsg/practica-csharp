using CustomerReaderAndProcessor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    public class CustomerFileReader
    {
        public List<Customer> ReadCustomers()
        {
            var customers = new List<Customer>();

            if (!File.Exists("file.data"))
                return customers;

            using (StreamReader reader = new StreamReader("file.data"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int id))
                    {
                        customers.Add(new Customer(id, parts[1]));
                    }
                }
            }

            return customers;
        }
    }
}
