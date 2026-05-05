using System.Windows;
using System.Windows.Input;
using Day14.Models;

namespace Day14.Views
{
    public partial class BookingDialog : Window
    {
        private MovieSession _session;

        public string CustomerName { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public int TicketCount { get; private set; }
        public MovieSession BookedSession { get; private set; }

        public BookingDialog(MovieSession session)
        {
            _session = session;
            BookedSession = session;
            InitializeComponent();
            DataContext = session;
        }

        private void DecrementTickets(object sender, RoutedEventArgs e)
        {
            if (_session.SelectedTickets > 1)
                _session.SelectedTickets--;
        }

        private void IncrementTickets(object sender, RoutedEventArgs e)
        {
            if (_session.SelectedTickets < _session.AvailableSeats)
                _session.SelectedTickets++;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CustomerNameBox.Text))
            {
                MessageBox.Show("Введите ФИО клиента", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                CustomerNameBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(PhoneBox.Text))
            {
                MessageBox.Show("Введите номер телефона", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PhoneBox.Focus();
                return;
            }

            CustomerName = CustomerNameBox.Text.Trim();
            PhoneNumber = PhoneBox.Text.Trim();
            TicketCount = _session.SelectedTickets;

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) =>
            DialogResult = false;
    }
}