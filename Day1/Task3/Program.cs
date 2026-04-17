int n;
Console.Write("Введите N (1<=N<=20): ");
n = int.Parse(Console.ReadLine());

double sum = 0;
for (int i = 1; i <= n; i++)
{
    sum += 1.0 / i;
}

Console.WriteLine($"Сумма: {sum:F4}");