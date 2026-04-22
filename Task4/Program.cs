using System;
namespace Task4
{
    class Program
    {
        static void Main()
        {
            Movie[] movies = new Movie[]
            {
                new Movie { Title = "Начало", Director = "Нолан", Duration = 148, Genre = "Фантастика" },
                new Movie { Title = "Тёмный рыцарь", Director = "Нолан", Duration = 152, Genre = "Боевик" },
                new Movie { Title = "Форрест Гамп", Director = "Земекис", Duration = 142, Genre = "Драма" }
            };

            Cinema cinema = new Cinema(movies);
            var longest = cinema.GetLongestMovie();
            Console.WriteLine("Самый длинный фильм:");
            longest.DisplayInfo();

            Console.WriteLine("\nФильмы Нолана:");
            var nolanMovies = cinema.GetMoviesByDirector("Нолан");
            foreach (var m in nolanMovies)
                m.DisplayInfo();
        }
    }
}

