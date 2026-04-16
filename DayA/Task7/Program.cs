using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите v0: ");
        double v0 = double.Parse(Console.ReadLine());

        Console.Write("Введите a: ");
        double a = double.Parse(Console.ReadLine());

        Console.Write("Введите t: ");
        double t = double.Parse(Console.ReadLine());

        double v = v0 + a * t;
        double s = v0 * t + (a * t * t) / 2;

        Console.WriteLine($"Скорость: {v:F2} м/с");
        Console.WriteLine($"Расстояние: {s:F2} м");
    }
}