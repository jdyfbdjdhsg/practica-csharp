using System;
using System.IO;

public class DeserializationException : Exception
{
    public DeserializationException() : base() { }

    public DeserializationException(string message) : base(message) { }

    public DeserializationException(string message, Exception innerException) : base(message, innerException) { }
}

public class XmlDeserializer
{
    public void Deserialize(string xml)
    {
        if (string.IsNullOrEmpty(xml) || !xml.StartsWith("<") || !xml.EndsWith(">"))
        {
            throw new InvalidOperationException("XML имеет неверный формат: отсутствуют открывающие или закрывающие теги");
        }

        Console.WriteLine("XML успешно десериализован");
    }
}

public class XmlProcessor
{
    public void ProcessXml(string xml)
    {
        try
        {
            XmlDeserializer deserializer = new XmlDeserializer();
            deserializer.Deserialize(xml);
        }
        catch (InvalidOperationException ex)
        {
            LogException(ex);

            throw new DeserializationException("Ошибка при десериализации XML", ex);
        }
    }

    private void LogException(Exception ex)
    {
        Console.WriteLine($"ЛОГ ИСКЛЮЧЕНИЯ");
        Console.WriteLine($"Тип исключения: {ex.GetType().Name}");
        Console.WriteLine($"Сообщение: {ex.Message}");
        Console.WriteLine($"Стек вызовов: {ex.StackTrace}");

        if (ex.InnerException != null)
        {
            Console.WriteLine($"Внутреннее исключение: {ex.InnerException.GetType().Name}");
            Console.WriteLine($"Сообщение внутреннего: {ex.InnerException.Message}");
        }
    }
}

class Program
{
    static void Main()
    {
        XmlProcessor processor = new XmlProcessor();

        try
        {
            processor.ProcessXml("Это не XML");
        }
        catch (DeserializationException ex)
        {
            Console.WriteLine($"Перехвачено в Main: {ex.Message}");
            Console.WriteLine($"Внутреннее исключение: {ex.InnerException?.Message}");
        }

        try
        {
            processor.ProcessXml("<root>data</root>");
        }
        catch (DeserializationException ex)
        {
            Console.WriteLine($"Перехвачено в Main: {ex.Message}");
        }
    }
}