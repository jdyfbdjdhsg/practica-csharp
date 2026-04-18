using System;
using System.Text.RegularExpressions;

class Task6
{
    static void Main()
    {
        string text = "abc 12345 def 678 ghi 9jkl";
        Console.WriteLine($"Исходная строка: {text}");

        string result = FindFirstDigitWord(text);

        if (result != null)
            Console.WriteLine($"Первое слово, состоящее только из цифр: {result}");
        else
            Console.WriteLine("Слова, состоящего только из цифр, не найдено");
    }

    static string FindFirstDigitWord(string str)
    {
        string[] words = str.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string word in words)
        {
            bool onlyDigits = true;
            foreach (char c in word)
            {
                if (!char.IsDigit(c))
                {
                    onlyDigits = false;
                    break;
                }
            }
            if (onlyDigits && word.Length > 0)
                return word;
        }

        return null;
    }
}