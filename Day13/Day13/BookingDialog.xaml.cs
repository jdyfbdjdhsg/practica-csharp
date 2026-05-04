using System.Windows;

namespace Day13
{
    public partial class BookingDialog : Window
    {
        public string Customer { get; private set; } = string.Empty;
        public string Movie { get; private set; } = string.Empty;
        public string Seat { get; private set; } = string.Empty;

        public BookingDialog()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }

        public BookingDialog(Booking existing) : this()
        {
            CustomerBox.Text = existing.Customer;
            MovieBox.Text = existing.Movie;
            SeatBox.Text = existing.Seat;
            Title = "Изменить бронь";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CustomerBox.Text))
            {
                MessageBox.Show("Введите имя клиента.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                CustomerBox.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(MovieBox.Text))
            {
                MessageBox.Show("Выберите или введите фильм.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                MovieBox.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(SeatBox.Text))
            {
                MessageBox.Show("Выберите или введите место.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                SeatBox.Focus(); return;
            }

            Customer = CustomerBox.Text.Trim();
            Movie = MovieBox.Text.Trim();
            Seat = SeatBox.Text.Trim();
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) =>
            DialogResult = false;
    }
}