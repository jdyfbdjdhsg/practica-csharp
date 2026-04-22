using System;
using System.Linq;

abstract class Order
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
}

sealed class OnlineOrder : Order { }
sealed class InStoreOrder : Order { }

class Store
{
    private Order[] orders;

    public Store(Order[] orders)
    {
        this.orders = orders;
    }

    public Order GetLargestOrder()
    {
        return orders.OrderByDescending(o => o.TotalAmount).FirstOrDefault();
    }

    public Order[] GetOrdersByCustomer(string customerName)
    {
        return orders.Where(o => o.CustomerName == customerName).ToArray();
    }
}

class Program3
{
    static void Main()
    {
        Order[] orders = new Order[]
        {
            new OnlineOrder { OrderId = 1, CustomerName = "Иван", TotalAmount = 5000 },
            new InStoreOrder { OrderId = 2, CustomerName = "Мария", TotalAmount = 12000 },
            new OnlineOrder { OrderId = 3, CustomerName = "Иван", TotalAmount = 3000 }
        };

        Store store = new Store(orders);
        var largest = store.GetLargestOrder();
        Console.WriteLine($"Самый дорогой заказ: {largest?.CustomerName} - {largest?.TotalAmount}");

        var ivanOrders = store.GetOrdersByCustomer("Иван");
        Console.WriteLine("Заказы Ивана:");
        foreach (var o in ivanOrders)
            Console.WriteLine($"  Id:{o.OrderId}, сумма:{o.TotalAmount}");
    }
}