double x;
Console.Write("Введите x: ");
x = double.Parse(Console.ReadLine());

double y;
if (x <= Math.PI)
{
    y = x + 2 * x * Math.Sin(3 * x);
}
else
{
    y = Math.Cos(x) + 2;
}

Console.WriteLine($"y = {y:F4}");