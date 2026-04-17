double a, b, c;
Console.Write("Введите сторону a: ");
a = double.Parse(Console.ReadLine());
Console.Write("Введите сторону b: ");
b = double.Parse(Console.ReadLine());
Console.Write("Введите сторону c: ");
c = double.Parse(Console.ReadLine());

bool exists = (a + b > c) && (a + c > b) && (b + c > a);
Console.WriteLine($"Треугольник существует: {exists}");