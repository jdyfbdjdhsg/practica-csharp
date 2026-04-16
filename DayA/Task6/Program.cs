using System;

class Program
{
    static void Main()
    {
        double x = 2.7;

        double y = Math.Log(x + Math.Sqrt(x * x + 9)) - (x + 1) / Math.Atan(x * x * x);

        Console.WriteLine($"y = {y:F8}");
    }
}