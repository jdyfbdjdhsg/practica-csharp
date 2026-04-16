using System;

class Program
{
    static void Main()
    {
        Console.Write("-> ");
        int minutes = int.Parse(Console.ReadLine());

        int hours = minutes / 60;
        int mins = minutes % 60;

        Console.WriteLine($"{minutes} минут — это {hours}ч. {mins} мин.");
    }
}