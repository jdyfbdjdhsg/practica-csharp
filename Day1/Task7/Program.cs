double kurs;
Console.Write("Введите курс доллара: ");
kurs = double.Parse(Console.ReadLine());

Console.WriteLine("Способ 1 (for):");
for (int dollars = 5; dollars <= 500; dollars += 5)
{
    Console.WriteLine($"{dollars} $ = {dollars * kurs:F2} руб.");
}

Console.WriteLine("\nСпособ 2 (while):");
int d = 5;
while (d <= 500)
{
    Console.WriteLine($"{d} $ = {d * kurs:F2} руб.");
    d += 5;
}

Console.WriteLine("\nСпособ 3 (do while):");
int d2 = 5;
do
{
    Console.WriteLine($"{d2} $ = {d2 * kurs:F2} руб.");
    d2 += 5;
} while (d2 <= 500);