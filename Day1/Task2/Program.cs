
namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            int num;
            Console.Write("Введите четырехзначное число: ");
            num = int.Parse(Console.ReadLine());

            int a = num / 1000;
            int b = (num / 100) % 10;
            int c = (num / 10) % 10;
            int d = num % 10;

            bool isPalindrome = (a == d && b == c);
            Console.WriteLine($"Число читается одинаково: {isPalindrome}");
        }
    }
}