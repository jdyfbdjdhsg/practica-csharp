double A = 0;
double B = Math.PI / 2;
int M = 20;
double H = (B - A) / M;

Console.WriteLine($"Табуляция функции F(x) = sin(x) - cos(x)");
Console.WriteLine($"На отрезке [{A}, {B}] с шагом {H:F4}\n");
Console.WriteLine("x\t\tF(x)");

double x = A;
for (int i = 0; i <= M; i++)
{
    double fx = Math.Sin(x) - Math.Cos(x);
    Console.WriteLine($"{x:F4}\t\t{fx:F4}");
    x += H;
}