namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {

            double r;
            Console.Write("Введите радиус окружности: ");
            r = double.Parse(Console.ReadLine());

            double length = 2 * Math.PI * r;
            double area = Math.PI * r * r;

            Console.WriteLine($"Длина окружности: {length:F4}");
            Console.WriteLine($"Площадь круга: {area:F4}");
        }
    }
}