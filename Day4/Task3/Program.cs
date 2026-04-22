using System;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = { 5, 3, 9, 1, 7, 2, 8 };
            int min = FindMin(array, array.Length);

            Console.WriteLine("Массив: " + string.Join(", ", array));
            Console.WriteLine($"Минимальный элемент: {min}");
        }

        static int FindMin(int[] array, int n)
        {
            if (n == 1) return array[0];
            int minOfRest = FindMin(array, n - 1);
            return array[n - 1] < minOfRest ? array[n - 1] : minOfRest;
        }
    }
}