using CustomerReaderAndProcessor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Task4
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
