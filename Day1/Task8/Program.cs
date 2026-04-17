int pricePerKg;
Console.Write("Введите цену 1 кг конфет (1-100): ");
pricePerKg = int.Parse(Console.ReadLine());

for (int kg = 1; kg <= 10; kg++)
{
    Console.WriteLine($"{kg} кг = {pricePerKg * kg} руб.");
}