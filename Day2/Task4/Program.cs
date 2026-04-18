using System;

class Task4
{
    static void Main()
    {
        int[,] array = {
            {1, 2, 3, 4},
            {5, 6, 7, 8},
            {9, 10, 11, 12},
            {13, 14, 15, 16}
        };

        Console.WriteLine("Исходный массив:");
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Console.Write($"{array[i, j],4}");
            }
            Console.WriteLine();
        }

        int sumFirst = 0;
        int sumPreLast = 0;

        for (int j = 0; j < array.GetLength(1); j++)
        {
            sumFirst += array[0, j];
        }

        int preLastRow = array.GetLength(0) - 2;
        for (int j = 0; j < array.GetLength(1); j++)
        {
            sumPreLast += array[preLastRow, j];
        }

        Console.WriteLine($"\nСумма первой строки: {sumFirst}");
        Console.WriteLine($"Сумма предпоследней строки: {sumPreLast}");

        if (sumFirst > sumPreLast)
            Console.WriteLine("Сумма элементов больше в первой строке");
        else if (sumPreLast > sumFirst)
            Console.WriteLine("Сумма элементов больше в предпоследней строке");
        else
            Console.WriteLine("Суммы равны");
    }
}