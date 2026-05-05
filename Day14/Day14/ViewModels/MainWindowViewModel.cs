using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Day14.Models;
using Day14.Views;

namespace Day14.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MovieSession> Sessions { get; set; }
        public ObservableCollection<Booking> Bookings { get; set; }

        private MovieSession? _selectedSession;
        public MovieSession? SelectedSession
        {
            get => _selectedSession;
            set { _selectedSession = value; OnPropertyChanged(); }
        }

        private Booking? _selectedBooking;
        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set { _selectedBooking = value; OnPropertyChanged(); }
        }

        private string _selectedFilter = "Все";
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();
                FilterSessions();
            }
        }

        public List<string> FilterOptions { get; } = new() { "Все", "Утро (до 12:00)", "День (12:00-18:00)", "Вечер (после 18:00)" };

        public ObservableCollection<MovieSession> FilteredSessions { get; set; }

        public ICommand BookTicketCommand { get; }
        public ICommand CancelBookingCommand { get; }

        public MainWindowViewModel()
        {
            Sessions = new ObservableCollection<MovieSession>();
            FilteredSessions = new ObservableCollection<MovieSession>();
            Bookings = new ObservableCollection<Booking>();

            LoadTestData();

            BookTicketCommand = new RelayCommand(_ => BookTicket(), _ => SelectedSession != null && SelectedSession.AvailableSeats > 0);
            CancelBookingCommand = new RelayCommand(_ => CancelBooking(), _ => SelectedBooking != null);
        }

        private void LoadTestData()
        {
            Sessions.Add(new MovieSession { Id = 1, Title = "Аватар 2", Time = "10:00", AvailableSeats = 50, Price = 400 });
            Sessions.Add(new MovieSession { Id = 2, Title = "Барби", Time = "12:30", AvailableSeats = 45, Price = 350 });
            Sessions.Add(new MovieSession { Id = 3, Title = "Оппенгеймер", Time = "15:45", AvailableSeats = 60, Price = 450 });
            Sessions.Add(new MovieSession { Id = 4, Title = "Джон Уик 4", Time = "19:00", AvailableSeats = 40, Price = 500 });
            Sessions.Add(new MovieSession { Id = 5, Title = "Дюна 2", Time = "21:30", AvailableSeats = 55, Price = 550 });

            FilterSessions();
        }

        private void FilterSessions()
        {
            FilteredSessions.Clear();

            var filtered = SelectedFilter switch
            {
                "Утро (до 12:00)" => Sessions.Where(s => int.Parse(s.Time.Split(':')[0]) < 12),
                "День (12:00-18:00)" => Sessions.Where(s => int.Parse(s.Time.Split(':')[0]) >= 12 && int.Parse(s.Time.Split(':')[0]) < 18),
                "Вечер (после 18:00)" => Sessions.Where(s => int.Parse(s.Time.Split(':')[0]) >= 18),
                _ => Sessions
            };

            foreach (var session in filtered)
                FilteredSessions.Add(session);
        }

        private void BookTicket()
        {
            if (SelectedSession == null) return;

            var dialog = new BookingDialog(SelectedSession);
            dialog.Owner = Application.Current.MainWindow;

            if (dialog.ShowDialog() == true)
            {
                var booking = new Booking
                {
                    Id = Bookings.Count + 1,
                    CustomerName = dialog.CustomerName,
                    PhoneNumber = dialog.PhoneNumber,
                    Session = dialog.BookedSession,
                    TicketCount = dialog.TicketCount
                };

                Bookings.Add(booking);

                SelectedSession.AvailableSeats -= dialog.TicketCount;
                SelectedSession.SelectedTickets = 1;

                MessageBox.Show($"Билеты успешно забронированы!\nСумма к оплате: {booking.TotalPrice} руб.",
                    "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CancelBooking()
        {
            if (SelectedBooking == null) return;

            var result = MessageBox.Show($"Отменить бронь для {SelectedBooking.CustomerName}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (SelectedBooking.Session != null)
                {
                    SelectedBooking.Session.AvailableSeats += SelectedBooking.TicketCount;
                }

                Bookings.Remove(SelectedBooking);
                MessageBox.Show("Бронь отменена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}