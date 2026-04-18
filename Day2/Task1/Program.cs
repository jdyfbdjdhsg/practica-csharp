using System;

class Task1
{
    static void Main()
    {
        int[] A = { -5, 3, -2, 7, -1, 0, -8, 4 };
        int count = 0;

        foreach (int num in A)
        {
            if (num < 0)
                count++;
        }

        Console.WriteLine($"Массив: [{string.Join(", ", A)}]");
        Console.WriteLine($"Количество отрицательных элементов: {count}");
    }
}