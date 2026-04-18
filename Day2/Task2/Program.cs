using System;
using System.Collections.Generic;
using System.Linq;

class Task2
{
    static void Main()
    {
        int[] a = new int[99];
        Random rand = new Random();

        for (int i = 0; i < 99; i++)
        {
            a[i] = rand.Next(-100, 101);
        }

        Console.WriteLine("Исходный массив (первые 20 элементов):");
        for (int i = 0; i < 20 && i < 99; i++)
        {
            Console.Write($"{a[i],5}");
        }
        Console.WriteLine("\n");

        int max = a[0];
        int min = a[0];
        int maxIndex = 0;
        int minIndex = 0;

        for (int i = 0; i < 99; i++)
        {
            if (a[i] > max)
            {
                max = a[i];
                maxIndex = i;
            }
            if (a[i] < min)
            {
                min = a[i];
                minIndex = i;
            }
        }

        Console.WriteLine($"Максимальный элемент: {max} (индекс {maxIndex})");
        Console.WriteLine($"Минимальный элемент: {min} (индекс {minIndex})");

        Console.Write("\nВведите номер элемента k (0-98), который нужно удалить: ");
        int k = int.Parse(Console.ReadLine());

        List<int> newSequence = new List<int>();
        bool maxRemoved = false;
        bool minRemoved = false;
        bool kRemoved = false;

        for (int i = 0; i < 99; i++)
        {
            if (a[i] == max && !maxRemoved)
            {
                maxRemoved = true;
                continue;
            }
            if (a[i] == min && !minRemoved)
            {
                minRemoved = true;
                continue;
            }
            if (i == k && !kRemoved)
            {
                kRemoved = true;
                continue;
            }
            newSequence.Add(a[i]);
        }

        Console.WriteLine($"\nУдалены: максимальный ({max}), минимальный ({min}) и элемент с индексом {k}");
        Console.WriteLine($"Размер новой последовательности: {newSequence.Count}");
        Console.WriteLine("\nНовая последовательность (первые 20 элементов):");
        for (int i = 0; i < 20 && i < newSequence.Count; i++)
        {
            Console.Write($"{newSequence[i],5}");
        }
        Console.WriteLine();
    }
}