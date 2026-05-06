using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Day16.Models;
using Day16.Services;

namespace Day16.Views
{
    public partial class MainWindow : Window
    {
        private readonly ICinemaService _cinemaService;
        private readonly IAuthService _authService;
        private User? _currentUser;
        private Movie? _selectedMovie;

        // Статическое свойство для передачи пользователя между окнами
        public static User? LoggedInUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            _authService = new AuthService();
            _cinemaService = new CinemaService(); // Убрали параметр null

            Loaded += async (s, e) => await InitializeAsync();

            RefreshMoviesButton.Click += async (s, e) => await LoadMoviesAsync();
            RefreshBookingsButton.Click += async (s, e) => await LoadBookingsAsync();
            CancelBookingButton.Click += async (s, e) => await CancelBookingAsync();
            LogoutButton.Click += Logout;

            MoviesListBox.SelectionChanged += async (s, e) =>
            {
                if (MoviesListBox.SelectedItem is Movie movie)
                {
                    _selectedMovie = movie;
                    await LoadSeatsAsync(movie.Id);
                }
            };
        }

        private async Task InitializeAsync()
        {
            try
            {
                // Сначала пробуем получить пользователя из статического свойства
                if (LoggedInUser != null)
                {
                    _currentUser = LoggedInUser;
                }
                else
                {
                    _currentUser = await _authService.GetCurrentUserAsync();
                }

                if (_currentUser != null)
                {
                    UserNameText.Text = _currentUser.DisplayName;
                    StatusText.Text = $"Добро пожаловать, {_currentUser.DisplayName}!";
                }
                else
                {
                    var loginWindow = new LoginWindow();
                    loginWindow.Show();
                    Close();
                    return;
                }

                await LoadMoviesAsync();
                await LoadBookingsAsync();
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Ошибка: {ex.Message}";
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadMoviesAsync()
        {
            try
            {
                LoadingProgressBar.Visibility = Visibility.Visible;
                StatusText.Text = "Загрузка фильмов...";

                var movies = await _cinemaService.GetMoviesAsync();

                MoviesListBox.Items.Clear();
                foreach (var movie in movies)
                {
                    MoviesListBox.Items.Add(movie);
                }

                StatusText.Text = $"Загружено {movies.Count} фильмов";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Ошибка: {ex.Message}";
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private async Task LoadSeatsAsync(int movieId)
        {
            try
            {
                LoadingProgressBar.Visibility = Visibility.Visible;
                StatusText.Text = "Загрузка схемы зала...";

                var seats = await _cinemaService.GetSeatsAsync(movieId);

                var mainPanel = new StackPanel();
                var groupedSeats = seats.GroupBy(s => s.Row).OrderBy(g => g.Key);

                foreach (var group in groupedSeats)
                {
                    var rowLabel = new TextBlock
                    {
                        Text = group.Key,
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 14,
                        Margin = new Thickness(0, 10, 0, 5)
                    };
                    mainPanel.Children.Add(rowLabel);

                    var seatsPanel = new WrapPanel { HorizontalAlignment = HorizontalAlignment.Center };

                    foreach (var seat in group.OrderBy(s => s.Number))
                    {
                        var button = new Button
                        {
                            Width = 45,
                            Height = 45,
                            Margin = new Thickness(3),
                            Content = new TextBlock
                            {
                                Text = seat.SeatNumber,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 12
                            },
                            Tag = seat,
                            Background = seat.IsAvailable ? new SolidColorBrush(Color.FromRgb(76, 175, 80)) : new SolidColorBrush(Color.FromRgb(244, 67, 54)),
                            IsEnabled = seat.IsAvailable
                        };

                        button.Click += async (s, e) => await BookSeatAsync(seat);
                        seatsPanel.Children.Add(button);
                    }

                    mainPanel.Children.Add(seatsPanel);
                }

                SeatsScrollViewer.Content = mainPanel;

                var availableCount = seats.Count(s => s.IsAvailable);
                StatusText.Text = $"Свободных мест: {availableCount} из {seats.Count}";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Ошибка: {ex.Message}";
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private async Task BookSeatAsync(Seat seat)
        {
            if (_currentUser == null)
            {
                StatusText.Text = "Ошибка: пользователь не авторизован";
                MessageBox.Show("Пожалуйста, войдите в систему заново.", "Ошибка авторизации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_selectedMovie == null)
            {
                StatusText.Text = "Сначала выберите фильм";
                MessageBox.Show("Пожалуйста, выберите фильм из списка.", "Нет выбора фильма",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            LoadingProgressBar.Visibility = Visibility.Visible;
            StatusText.Text = "Бронирование...";

            var booking = new Booking
            {
                UserId = _currentUser.Id,
                CustomerName = _currentUser.DisplayName,
                PhoneNumber = _currentUser.PhoneNumber ?? "",
                MovieId = _selectedMovie.Id,
                MovieTitle = _selectedMovie.Title,
                SeatNumber = seat.SeatNumber,
                Price = seat.Price
            };

            try
            {
                var result = await _cinemaService.BookSeatAsync(booking);

                if (result != null)
                {
                    await LoadSeatsAsync(_selectedMovie.Id);
                    await LoadBookingsAsync();
                    StatusText.Text = "Билет успешно забронирован!";

                    MessageBox.Show($"Бронирование успешно!\n\nФильм: {_selectedMovie.Title}\nМесто: {seat.SeatNumber}\nСумма: {seat.Price} руб.",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    StatusText.Text = "Место уже занято";
                    MessageBox.Show("Это место уже забронировано. Выберите другое место.",
                        "Место занято", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Ошибка: {ex.Message}";
                MessageBox.Show($"Ошибка при бронировании: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private async Task LoadBookingsAsync()
        {
            try
            {
                if (_currentUser != null)
                {
                    var bookings = await _cinemaService.GetBookingsAsync(_currentUser.Id);
                    BookingsListBox.Items.Clear();
                    foreach (var booking in bookings)
                    {
                        BookingsListBox.Items.Add(booking);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Ошибка: {ex.Message}";
            }
        }

        private async Task CancelBookingAsync()
        {
            if (BookingsListBox.SelectedItem is Booking selectedBooking)
            {
                var result = MessageBox.Show($"Отменить бронь для {selectedBooking.CustomerName}?\n\nФильм: {selectedBooking.MovieTitle}\nМесто: {selectedBooking.SeatNumber}",
                    "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    LoadingProgressBar.Visibility = Visibility.Visible;
                    StatusText.Text = "Отмена бронирования...";

                    try
                    {
                        var success = await _cinemaService.CancelBookingAsync(selectedBooking.Id);

                        if (success)
                        {
                            await LoadBookingsAsync();
                            if (_selectedMovie != null)
                            {
                                await LoadSeatsAsync(_selectedMovie.Id);
                            }
                            StatusText.Text = "Бронь отменена";

                            MessageBox.Show("Бронирование успешно отменено.", "Отмена",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        StatusText.Text = $"Ошибка: {ex.Message}";
                    }
                    finally
                    {
                        LoadingProgressBar.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                StatusText.Text = "Выберите бронь для отмены";
                MessageBox.Show("Пожалуйста, выберите бронь из списка для отмены.",
                    "Нет выбора", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void Logout(object sender, RoutedEventArgs e)
        {
            await _authService.LogoutAsync();
            LoggedInUser = null;

            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}