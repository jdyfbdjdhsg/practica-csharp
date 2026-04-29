using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CinemaBooking
{
    public partial class MainWindow : Window
    {
        private List<Button> allSeats = new List<Button>();
        private Button selectedSeat = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSeats();
        }

        private void InitializeSeats()
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    Button seat = new Button();
                    seat.Width = 40;
                    seat.Height = 40;
                    seat.Margin = new Thickness(5);
                    seat.Content = $"{row + 1}{(char)('А' + col)}"; // Номер места: 1А, 1Б и т.д.
                    seat.Background = Brushes.Gray;

                    if ((row == 1 && col == 2) || (row == 2 && col == 3) || (row == 3 && col == 1))
                    {
                        seat.Background = Brushes.Red;
                        seat.IsEnabled = false;
                    }

                    seat.Click += Seat_Click;

                    Grid.SetRow(seat, row);
                    Grid.SetColumn(seat, col);
                    SeatsGrid.Children.Add(seat);
                    allSeats.Add(seat);
                }
            }
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            Button clickedSeat = sender as Button;

            if (clickedSeat.Background == Brushes.Red || MovieComboBox.SelectedIndex == -1)
            {
                if (MovieComboBox.SelectedIndex == -1)
                    MessageBox.Show("Пожалуйста, сначала выберите фильм!", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedSeat != null)
            {
                if (selectedSeat.Background != Brushes.Red)
                    selectedSeat.Background = Brushes.Gray;
            }

            selectedSeat = clickedSeat;
            selectedSeat.Background = Brushes.Green;
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            if (MovieComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите фильм!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedSeat == null)
            {
                MessageBox.Show("Выберите место!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string movie = (MovieComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            string seatNumber = selectedSeat.Content.ToString();

            MessageBoxResult result = MessageBox.Show(
                $"Фильм: {movie}\nМесто: {seatNumber}\n\nЗабронировать?",
                "Подтверждение бронирования",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                selectedSeat.Background = Brushes.Red;
                selectedSeat.IsEnabled = false;
                selectedSeat = null;

                MessageBox.Show("Билет успешно забронирован!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}