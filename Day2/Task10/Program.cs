using System;
using System.Text.RegularExpressions;

class Task10
{
    static void Main()
    {
        string[] testStrings = {
            "Hello World",
            "Привет Мир",
            "Hello Привет",
            "123456",
            "Русский текст",
            "English text"
        };

        foreach (string str in testStrings)
        {
            Console.WriteLine($"Строка: '{str}'");
            Console.WriteLine($"Содержит русские буквы: {ContainsRussianLetters(str)}");
            Console.WriteLine();
        }
    }

    static bool ContainsRussianLetters(string str)
    {
        Regex regex = new Regex(@"[А-Яа-яЁё]");
        return regex.IsMatch(str);
    }
}