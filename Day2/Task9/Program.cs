using System;
using System.Text;

class Task9
{
    static void Main()
    {
        StringBuilder sb = new StringBuilder("Hello StringBuilder");
        Console.WriteLine($"Исходный StringBuilder: {sb}");
        Console.WriteLine($"Тип: {sb.GetType()}");

        string str = sb.ToString();
        Console.WriteLine($"\nПреобразовано в string: {str}");
        Console.WriteLine($"Тип: {str.GetType()}");

        StringBuilder sb2 = new StringBuilder(str);
        Console.WriteLine($"\nПреобразовано обратно в StringBuilder: {sb2}");
        Console.WriteLine($"Тип: {sb2.GetType()}");

        sb2.Append(" - modified");
        Console.WriteLine($"\nПосле изменения: {sb2}");
    }
}