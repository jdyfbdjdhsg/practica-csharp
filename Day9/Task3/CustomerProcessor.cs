
namespace Task3
{
    public class CustomerProcessor
    {
        public List<DuplicateInfo> FindDuplicates(List<Customer> customers)
        {
            var duplicates = customers
                .GroupBy(c => c.Id)
                .Where(g => g.Count() > 1)
                .Select(g => new DuplicateInfo
                {
                    Id = g.Key,
                    Names = g.Select(c => c.Name).ToList()
                })
                .ToList();

            return duplicates;
        }
    }
}
