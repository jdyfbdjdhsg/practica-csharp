using System;

public class InvalidEmailFormatException : Exception
{
    public InvalidEmailFormatException() : base() { }

    public InvalidEmailFormatException(string message) : base(message) { }

    public InvalidEmailFormatException(string message, Exception innerException) : base(message, innerException) { }
}

public class EmailValidator
{
    public void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new InvalidEmailFormatException("Email не может быть пустым");

        if (!email.Contains("@") || !email.Contains("."))
            throw new InvalidEmailFormatException($"Некорректный формат email: {email}. Email должен содержать @ и .");
    }
}

class Program
{
    static void Main()
    {
        EmailValidator validator = new EmailValidator();

        try
        {
            validator.ValidateEmail("user@example.com");
            Console.WriteLine("Email корректен");
        }
        catch (InvalidEmailFormatException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            validator.ValidateEmail("userexample.com");
            Console.WriteLine("Email корректен");
        }
        catch (InvalidEmailFormatException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            validator.ValidateEmail("user@examplecom");
            Console.WriteLine("Email корректен");
        }
        catch (InvalidEmailFormatException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}