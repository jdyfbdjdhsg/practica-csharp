using System;
using System.Text;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "hello world from csharp programming";
            string result = text.FirstLettersOfWords();

            Console.WriteLine($"Исходная строка: \"{text}\"");
            Console.WriteLine($"Первые буквы слов: \"{result}\"");
        }
    }

    public static class StringExtensions
    {
        public static string FirstLettersOfWords(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new StringBuilder();

            foreach (string word in words)
            {
                if (word.Length > 0)
                    result.Append(word[0]);
            }
            return result.ToString();
        }
    }
}