using System.Windows;

namespace Day13
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Close();

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Система бронирования билетов в кинотеатр\nВерсия 1.0\n\n" +
                "Ctrl+N — забронировать\nCtrl+E — изменить бронь\nCtrl+D — отменить бронь",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}