using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Day14.Models
{
    public class MovieSession : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        private string _time = string.Empty;
        private int _availableSeats;
        private int _price;
        private int _selectedTickets = 1;

        public int Id { get; set; }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); }
        }

        public int AvailableSeats
        {
            get => _availableSeats;
            set { _availableSeats = value; OnPropertyChanged(); }
        }

        public int Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        public int SelectedTickets
        {
            get => _selectedTickets;
            set
            {
                if (value >= 1 && value <= AvailableSeats)
                {
                    _selectedTickets = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        public int TotalPrice => Price * SelectedTickets;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}