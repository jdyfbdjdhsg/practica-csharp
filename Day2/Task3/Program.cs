using System;

class Task3
{
    static void Main()
    {
        Console.Write("Введите N (N<10): ");
        int N = int.Parse(Console.ReadLine());

        Console.Write("Введите a: ");
        int a = int.Parse(Console.ReadLine());

        Console.Write("Введите b: ");
        int b = int.Parse(Console.ReadLine());

        Console.Write("Введите G: ");
        int G = int.Parse(Console.ReadLine());

        Console.Write("Введите k (номер строки): ");
        int k = int.Parse(Console.ReadLine());

        int[,] matrix = new int[N, N];
        Random rand = new Random();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                matrix[i, j] = rand.Next(a, b + 1);
            }
        }

        Console.WriteLine("\nИсходная матрица:");
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Console.Write($"{matrix[i, j],5}");
            }
            Console.WriteLine();
        }

        int sum = 0;
        int count = 0;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (matrix[i, j] > G)
                {
                    sum += matrix[i, j];
                    count++;
                }
            }
        }

        double average = count > 0 ? (double)sum / count : 0;
        Console.WriteLine($"\nСреднее арифметическое элементов > {G}: {average:F2}");

        if (k >= 0 && k < N)
        {
            int evenCount = 0;
            for (int j = 0; j < N; j++)
            {
                if (matrix[k, j] % 2 == 0)
                    evenCount++;
            }
            Console.WriteLine($"Количество четных элементов в строке {k}: {evenCount}");
        }
        else
        {
            Console.WriteLine("Неверный номер строки!");
        }
    }
}