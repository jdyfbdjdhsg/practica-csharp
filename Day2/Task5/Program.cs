using System;
using System.Collections.Generic;
using System.Linq;

class Task5
{
    static void Main()
    {
        int[][] jaggedArray = new int[][]
        {
            new int[] {1, 2, 3, 4},
            new int[] {4, 3, 2, 1},
            new int[] {2, 4, 1, 3},
            new int[] {1, 2, 3, 5}
        };

        Console.WriteLine("Ступенчатый массив:");
        for (int i = 0; i < jaggedArray.Length; i++)
        {
            Console.WriteLine($"Строка {i}: [{string.Join(", ", jaggedArray[i])}]");
        }

        bool allArePermutations = true;

        for (int i = 1; i < jaggedArray.Length; i++)
        {
            if (!ArePermutations(jaggedArray[0], jaggedArray[i]))
            {
                allArePermutations = false;
                break;
            }
        }

        if (allArePermutations)
            Console.WriteLine("\nВсе строки являются перестановками друг друга");
        else
            Console.WriteLine("\nНе все строки являются перестановками друг друга");
    }

    static bool ArePermutations(int[] arr1, int[] arr2)
    {
        if (arr1.Length != arr2.Length)
            return false;

        List<int> list1 = arr1.ToList();
        List<int> list2 = arr2.ToList();

        list1.Sort();
        list2.Sort();

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
                return false;
        }

        return true;
    }
}