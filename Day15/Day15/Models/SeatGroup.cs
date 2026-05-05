using System.Collections.Generic;

namespace Day15.Models
{
    public class SeatGroup
    {
        public string Row { get; set; } = string.Empty;
        public List<Seat> Seats { get; set; } = new List<Seat>();
    }
}