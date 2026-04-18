using System;
using System.Collections.Generic;

class Task8
{
    static void Main()
    {
        string text = "Hello world, hello C# programming hello world";
        List<string> badWords = new List<string> { "hello", "world" };

        Console.WriteLine($"Исходная строка: {text}");
        Console.WriteLine($"Список слов для замены: {string.Join(", ", badWords)}");

        string result = ReplaceWordsWithStars(text, badWords);
        Console.WriteLine($"Результат: {result}");
    }

    static string ReplaceWordsWithStars(string str, List<string> wordsToReplace)
    {
        string[] words = str.Split(' ');
        List<string> result = new List<string>();

        foreach (string word in words)
        {
            bool needReplace = false;
            string cleanWord = word.Trim(',', '.', '!', '?', ';', ':');

            foreach (string badWord in wordsToReplace)
            {
                if (string.Equals(cleanWord, badWord, StringComparison.OrdinalIgnoreCase))
                {
                    needReplace = true;
                    break;
                }
            }

            if (needReplace)
            {
                string stars = new string('*', word.Length);
                result.Add(stars);
            }
            else
            {
                result.Add(word);
            }
        }

        return string.Join(" ", result);
    }
}