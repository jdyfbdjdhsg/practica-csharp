using System;

namespace CinemaSystem
{
    class Movie
    {
        public string Title { get; set; }

        public Movie(string title)
        {
            Title = title;
        }
    }

    class Schedule
    {
        public string[] ShowTimes { get; set; }

        public Schedule()
        {
            ShowTimes = new string[] { "10:00", "13:00", "16:00", "19:00", "22:00" };
        }

        public void ShowSchedule()
        {
            Console.WriteLine("Расписание сеансов:");
            foreach (var time in ShowTimes)
            {
                Console.WriteLine($"  - {time}");
            }
        }
    }

    class Distributor
    {
        public string Name { get; set; }

        public Distributor(string name)
        {
            Name = name;
        }

        public void ProvideMovie(Movie movie)
        {
            Console.WriteLine($"Дистрибьютор {Name} предоставил фильм '{movie.Title}'");
        }
    }

    class Cinema
    {
        public string Name { get; set; }

        public Movie[] Movies { get; set; }

        public Schedule Schedule { get; private set; }

        public Distributor Distributor { get; set; }

        public Cinema(string name, Movie[] movies, Distributor distributor)
        {
            Name = name;
            Movies = movies;
            Distributor = distributor;
            Schedule = new Schedule();
        }

        public void ShowMovies()
        {
            Console.WriteLine($"\nКинотеатр: {Name}");
            Console.WriteLine($"Дистрибьютор: {Distributor.Name}");

            foreach (var movie in Movies)
            {
                Distributor.ProvideMovie(movie);
                Console.WriteLine($"Показ фильма: '{movie.Title}'");
                Schedule.ShowSchedule();
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Movie movie1 = new Movie("Аватар");
            Movie movie2 = new Movie("Интерстеллар");
            Movie movie3 = new Movie("Дюна");
            Movie movie4 = new Movie("Оппенгеймер");

            Distributor distributor1 = new Distributor("Universal Pictures");
            Distributor distributor2 = new Distributor("Warner Bros");

            Cinema[] cinemas = new Cinema[]
            {
                new Cinema("Космос", new Movie[] { movie1, movie2 }, distributor1),
                new Cinema("Россия", new Movie[] { movie3, movie4 }, distributor2),
                new Cinema("Заря", new Movie[] { movie1, movie3 }, distributor1)
            };

            foreach (var cinema in cinemas)
            {
                cinema.ShowMovies();
            }
        }
    }
}