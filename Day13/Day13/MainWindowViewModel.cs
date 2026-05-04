using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Day13
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Booking> Bookings { get; } = new();

        private Booking? _selectedBooking;
        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set { _selectedBooking = value; OnPropertyChanged(); }
        }

        public ICommand BookTicketCommand { get; }
        public ICommand EditTicketCommand { get; }
        public ICommand CancelTicketCommand { get; }

        public MainWindowViewModel()
        {
            BookTicketCommand = new RelayCommand(_ => BookTicket());
            EditTicketCommand = new RelayCommand(_ => EditTicket(), _ => SelectedBooking != null);
            CancelTicketCommand = new RelayCommand(_ => CancelTicket(), _ => SelectedBooking != null);
        }

        private void BookTicket()
        {
            var dialog = new BookingDialog();
            if (dialog.ShowDialog() == true)
            {
                Bookings.Add(new Booking
                {
                    Customer = dialog.Customer,
                    Movie = dialog.Movie,
                    Seat = dialog.Seat
                });
            }
        }

        private void EditTicket()
        {
            if (SelectedBooking == null) return;
            var dialog = new BookingDialog(SelectedBooking);
            if (dialog.ShowDialog() == true)
            {
                var index = Bookings.IndexOf(SelectedBooking);
                Bookings[index] = new Booking
                {
                    Customer = dialog.Customer,
                    Movie = dialog.Movie,
                    Seat = dialog.Seat
                };
                SelectedBooking = Bookings[index];
            }
        }

        private void CancelTicket()
        {
            if (SelectedBooking == null) return;
            var result = MessageBox.Show(
                $"Отменить бронь для \"{SelectedBooking.Customer}\"?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                Bookings.Remove(SelectedBooking);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}