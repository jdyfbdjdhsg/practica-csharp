using System;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            double A = 1.5, B = 2.7, C = 3.9, D = 4.2;

            Console.WriteLine($"До обмена: A={A}, B={B}, C={C}, D={D}");

            Swap(ref A, ref B);
            Swap(ref C, ref D);
            Swap(ref B, ref C);

            Console.WriteLine($"После обмена: A={A}, B={B}, C={C}, D={D}");
        }

        static void Swap(ref double X, ref double Y)
        {
            double temp = X;
            X = Y;
            Y = temp;
        }
    }
}