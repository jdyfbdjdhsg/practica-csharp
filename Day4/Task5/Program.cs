using System;

namespace Task5
{
    class Program
    {
        static void Main(string[] args)
        {
            Food pizza = new Pizza();
            Food pasta = new Pasta();

            pizza.Cook();
            pizza.Serve();

            pasta.Cook();
            pasta.Serve();
        }
    }

    public abstract class Food
    {
        public abstract void Cook();
        public virtual void Serve()
        {
            Console.WriteLine("Serving food");
        }
    }

    public class Pizza : Food
    {
        public override void Cook()
        {
            Console.WriteLine("Pizza is cooking");
        }

        public override void Serve()
        {
            Console.WriteLine("Serving pizza on a plate");
        }
    }

    public class Pasta : Food
    {
        public override void Cook()
        {
            Console.WriteLine("Pasta is cooking");
        }

        public override void Serve()
        {
            Console.WriteLine("Serving pasta in a bowl");
        }
    }
}