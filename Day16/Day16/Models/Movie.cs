namespace Day16.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int HallNumber { get; set; } = 1;
        public DateTime ShowDate { get; set; } = DateTime.Today;
    }
}