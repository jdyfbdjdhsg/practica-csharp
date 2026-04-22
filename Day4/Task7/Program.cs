using System;

class Program
{
    static void Main()
    {
        Console.WriteLine(LogMessage("Server started"));
        Console.WriteLine(LogMessage("Error occurred", "404"));
    }

    static string LogMessage(string message)
    {
        return message;
    }

    static string LogMessage(string message, string errorCode)
    {
        return message + ", Code: " + errorCode;
    }
}