using System;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "Hello world from C# programming language";
            int count = WordCount(text);
            Console.WriteLine($"Строка: \"{text}\"");
            Console.WriteLine($"Количество слов: {count}");
        }

        static int WordCount(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return 0;
            return str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
