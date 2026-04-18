using System;

class Task7
{
    static void Main()
    {
        string text = "abcabcbb";
        Console.WriteLine($"Исходная строка: {text}");

        int length = LongestUniqueSubstring(text);
        Console.WriteLine($"Длина самой длинной подстроки без повторяющихся символов: {length}");
    }

    static int LongestUniqueSubstring(string str)
    {
        int maxLength = 0;

        for (int i = 0; i < str.Length; i++)
        {
            bool[] seen = new bool[256];
            int currentLength = 0;

            for (int j = i; j < str.Length; j++)
            {
                if (seen[str[j]])
                    break;

                seen[str[j]] = true;
                currentLength++;
            }

            if (currentLength > maxLength)
                maxLength = currentLength;
        }

        return maxLength;
    }
}