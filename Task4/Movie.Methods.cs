using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Task4
{
    public partial class Movie
    {
        public void DisplayInfo()
        {
            Console.WriteLine($"{Title} ({Genre}) - {Duration} мин, реж. {Director}");
        }
    }

    public class Cinema
    {
        private Movie[] movies;

        public Cinema(Movie[] movies)
        {
            this.movies = movies;
        }

        public Movie GetLongestMovie()
        {
            return movies.OrderByDescending(m => m.Duration).FirstOrDefault();
        }

        public Movie[] GetMoviesByDirector(string director)
        {
            return movies.Where(m => m.Director == director).ToArray();
        }
    }
}