using System;

class Program
{
    static void Main()
    {
        int D1 = 120, D2 = 90, D3 = 150, D4 = 45, D5 = 180;

        DistanceToHours(D1, out int H1, out int M1);
        DistanceToHours(D2, out int H2, out int M2);
        DistanceToHours(D3, out int H3, out int M3);
        DistanceToHours(D4, out int H4, out int M4);
        DistanceToHours(D5, out int H5, out int M5);

        Console.WriteLine($"{D1} км = {H1} ч {M1} мин");
        Console.WriteLine($"{D2} км = {H2} ч {M2} мин");
        Console.WriteLine($"{D3} км = {H3} ч {M3} мин");
        Console.WriteLine($"{D4} км = {H4} ч {M4} мин");
        Console.WriteLine($"{D5} км = {H5} ч {M5} мин");
    }

    static void DistanceToHours(int KM, out int H, out int M)
    {
        H = KM / 60;
        M = KM % 60;
    }
}