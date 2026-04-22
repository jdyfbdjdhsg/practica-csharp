using System;

class Program
{
    static void Main()
    {
        ConvertCurrency(100, out double result1);
        ConvertCurrency(100, 1.2, out double result2);

        Console.WriteLine(result1);
        Console.WriteLine(result2);
    }

    static void ConvertCurrency(in double amount, out double convertedAmount)
    {
        convertedAmount = amount * 0.85;
    }

    static void ConvertCurrency(in double amount, double exchangeRate, out double convertedAmount)
    {
        convertedAmount = amount * exchangeRate;
    }
}