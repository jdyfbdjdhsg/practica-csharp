using System;
using System.Collections.Generic;

public class OutOfStockException : Exception
{
    public OutOfStockException() : base() { }

    public OutOfStockException(string message) : base(message) { }

    public OutOfStockException(string message, Exception innerException) : base(message, innerException) { }
}

public class Inventory
{
    private Dictionary<string, int> stock = new Dictionary<string, int>();

    public Inventory()
    {
        stock.Add("Телефон", 10);
        stock.Add("Ноутбук", 5);
        stock.Add("Наушники", 0);
    }

    public void CheckStock(string item)
    {
        if (string.IsNullOrEmpty(item))
            throw new OutOfStockException("Название товара не может быть пустым");

        if (!stock.ContainsKey(item))
            throw new OutOfStockException($"Товар '{item}' отсутствует на складе");

        if (stock[item] == 0)
            throw new OutOfStockException($"Товар '{item}' есть в системе, но отсутствует в наличии (количество: {stock[item]})");

        Console.WriteLine($"Товар '{item}' есть в наличии. Количество: {stock[item]}");
    }
}

class Program
{
    static void Main()
    {
        Inventory inventory = new Inventory();
       
        try
        {
            inventory.CheckStock("Планшет");
        }
        catch (OutOfStockException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            inventory.CheckStock("Наушники");
        }
        catch (OutOfStockException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            inventory.CheckStock("Телефон");
        }
        catch (OutOfStockException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            inventory.CheckStock("");
        }
        catch (OutOfStockException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}