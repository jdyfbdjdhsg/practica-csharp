using System;
using System.Collections.Generic;

namespace Task2_MyBag
{
    class MyBag<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public List<T> GetAll()
        {
            return items;
        }
    }

    class BagManager<T>
    {
        private MyBag<T> bag = new MyBag<T>();

        public void AddItem(T item)
        {
            bag.Add(item);
            Console.WriteLine($"Добавлено: {item}");
        }

        public void RemoveItem(T item)
        {
            if (bag.Remove(item))
                Console.WriteLine($"Удалено: {item}");
            else
                Console.WriteLine($"Не найдено: {item}");
        }

        public bool HasItem(T item)
        {
            return bag.Contains(item);
        }

        public void ShowAllItems()
        {
            Console.WriteLine("\nСодержимое сумки:");
            foreach (var item in bag.GetAll())
            {
                Console.WriteLine($"  {item}");
            }
        }
    }
    class Program
    {
        static void Main()
        {
            var manager = new BagManager<string>();

            manager.AddItem("Зелье здоровья");
            manager.AddItem("Стальной меч");
            manager.AddItem("Зелье здоровья");

            manager.ShowAllItems();

            Console.WriteLine($"Есть меч? {manager.HasItem("Стальной меч")}");
            Console.WriteLine($"Есть лук? {manager.HasItem("Лук")}");

            manager.RemoveItem("Зелье здоровья");
            manager.ShowAllItems();
        }
    }
}