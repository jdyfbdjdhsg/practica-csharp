using System;
using System.IO;

public delegate void FileHandler(string filePath);

public class FileReader
{
    public void ReadFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            Console.WriteLine($"Прочитано из файла: {content}");
        }
        else
        {
            Console.WriteLine($"Файл {filePath} не найден");
        }
    }
}

public class FileWriter
{
    public void WriteFile(string filePath)
    {
        string content = $"Запись создана: {DateTime.Now}";
        File.WriteAllText(filePath, content);
        Console.WriteLine($"Записано в файл: {content}");
    }
}

class Program
{
    static void Main()
    {
        string testFile = "test.txt";

        FileReader reader = new FileReader();
        FileWriter writer = new FileWriter();

        FileHandler handler;

        handler = writer.WriteFile;
        handler(testFile);

        handler = reader.ReadFile;
        handler(testFile);

        Console.WriteLine("\nМногоадресный делегат");
        handler = writer.WriteFile;
        handler += reader.ReadFile;
        handler(testFile);
    }
}