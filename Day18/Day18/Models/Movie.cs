using System.ComponentModel.DataAnnotations;

namespace Day18.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int HallNumber { get; set; } = 1;
        public DateTime ShowDate { get; set; } = DateTime.Today;
        public string PosterColor { get; set; } = "#FF2196F3";
    }
}