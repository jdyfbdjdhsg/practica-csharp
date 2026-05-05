using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Day14.Models
{
    public class Booking : INotifyPropertyChanged
    {
        private string _customerName = string.Empty;
        private string _phoneNumber = string.Empty;
        private MovieSession? _session;
        private int _ticketCount;

        public int Id { get; set; }

        public string CustomerName
        {
            get => _customerName;
            set { _customerName = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        public MovieSession? Session
        {
            get => _session;
            set { _session = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); }
        }

        public int TicketCount
        {
            get => _ticketCount;
            set { _ticketCount = value; OnPropertyChanged(); OnPropertyChanged(nameof(DisplayText)); OnPropertyChanged(nameof(TotalPrice)); }
        }

        public string DisplayText => $"{CustomerName} - {Session?.Title} ({Session?.Time}) - {TicketCount} бил. - {TotalPrice} руб.";

        public int TotalPrice => (Session?.Price ?? 0) * TicketCount;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}