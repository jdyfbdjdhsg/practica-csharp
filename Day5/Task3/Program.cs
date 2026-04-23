using System;
using System.Collections.Generic;

namespace SportsTeams
{
    abstract class TeamMember
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public TeamMember(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public abstract void Introduce();
    }

    interface IBasketballPlayer
    {
        void ShootThreePointer();
        void Dribble();
    }

    interface IFootballPlayer
    {
        void KickBall();
        void PassBall();
    }

    class Basketballer : TeamMember, IBasketballPlayer
    {
        public Basketballer(string name, int age) : base(name, age) { }

        public void ShootThreePointer()
        {
            Console.WriteLine($"{Name} выполняет трёхочковый бросок!");
        }

        public void Dribble()
        {
            Console.WriteLine($"{Name} ведёт мяч!");
        }

        public override void Introduce()
        {
            Console.WriteLine($"Баскетболист: {Name}, возраст: {Age}");
        }
    }

    class Footballer : TeamMember, IFootballPlayer
    {
        public Footballer(string name, int age) : base(name, age) { }

        public void KickBall()
        {
            Console.WriteLine($"{Name} бьёт по мячу!");
        }

        public void PassBall()
        {
            Console.WriteLine($"{Name} передаёт мяч партнёру!");
        }

        public override void Introduce()
        {
            Console.WriteLine($"Футболист: {Name}, возраст: {Age}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TeamMember[] players = new TeamMember[]
            {
                new Basketballer("Майкл Джордан", 35),
                new Footballer("Лионель Месси", 34),
                new Basketballer("Леброн Джеймс", 38),
                new Footballer("Криштиану Роналду", 37),
                new Basketballer("Стефен Карри", 34)
            };

            Console.WriteLine("Все игроки\n");
            foreach (var player in players)
            {
                player.Introduce();
            }

            Console.WriteLine("\nБаскетболисты\n");
            foreach (var player in players)
            {
                if (player is IBasketballPlayer basketballPlayer)
                {
                    Console.WriteLine($"Найден баскетболист: {player.Name}");
                    basketballPlayer.ShootThreePointer();
                    basketballPlayer.Dribble();
                    Console.WriteLine();
                }
            }
        }
    }
}