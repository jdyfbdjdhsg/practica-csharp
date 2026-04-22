using System;

class A
{
    public int a;
    public int b;

    public A(int a, int b)
    {
        this.a = a;
        this.b = b;
    }

    public int SquareOfSum()
    {
        int sum = a + b;
        return sum * sum;
    }
    public int SumOfSquares()
    {
        return a * a + b * b;
    }
}

class Program
{
    static void Main()
    {
        A obj = new A(3, 4);
        Console.WriteLine($"a = {obj.a}, b = {obj.b}");
        Console.WriteLine($"Квадрат суммы: {obj.SquareOfSum()}");
        Console.WriteLine($"Сумма квадратов: {obj.SumOfSquares()}");
    }
}