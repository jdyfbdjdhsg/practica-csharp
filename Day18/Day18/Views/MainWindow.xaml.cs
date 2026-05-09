using Day18.Data;
using Day18.Models;
using Day18.Services;
using Day18.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Day18.Views
{
    public partial class MainWindow : Window
    {
        private readonly ICinemaService _cinemaService;
        private readonly IAuthService _authService;
        private User? _currentUser;
        private Movie? _selectedMovie;
        private Grid? _popupOverlay;

        private CinemaViewModel? _cinemaViewModel;
        private AppDbContext _dbContext = null!;
        private SessionRepository _sessionRepository = null!;
        private TicketRepository _ticketRepository = null!;

        public static User? LoggedInUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) => await InitializeAsync();

            _authService = new AuthService();
            _cinemaService = new CinemaService();

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
                _currentUser = LoggedInUser ?? await _authService.GetCurrentUserAsync();

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

                // Инициализация для варианта 9 с SQLite
                _dbContext = new AppDbContext();
                _sessionRepository = new SessionRepository(_dbContext);
                _ticketRepository = new TicketRepository(_dbContext);

                _cinemaViewModel = new CinemaViewModel(_sessionRepository, _ticketRepository, _currentUser);

                // Подписываемся на события
                if (_cinemaViewModel != null)
                {
                    _cinemaViewModel.BookingCompleted += async (sender, e) =>
                    {
                        if (_selectedMovie != null)
                        {
                            await LoadSeatsAsync(_selectedMovie.Id);
                        }
                        await LoadBookingsAsync();
                    };

                    _cinemaViewModel.BookingCancelled += async (sender, e) =>
                    {
                        if (_selectedMovie != null)
                        {
                            await LoadSeatsAsync(_selectedMovie.Id);
                        }
                        await LoadBookingsAsync();
                    };
                }

                // Установка DataContext
                this.DataContext = _cinemaViewModel;

                // Загрузка данных для варианта 9
                if (_cinemaViewModel != null)
                {
                    await _cinemaViewModel.LoadSessionsAsync();
                    await _cinemaViewModel.LoadTicketsAsync();
                }

                // Загрузка старых данных
                await LoadMoviesAsync();
                await LoadBookingsAsync();

                // Создание мест в БД если их нет
                await InitializeSeatsIfNeeded();
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Ошибка: {ex.Message}";
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task InitializeSeatsIfNeeded()
        {
            var seatsCount = await _dbContext.Seats.CountAsync();
            if (seatsCount == 0)
            {
                var movies = await _cinemaService.GetMoviesAsync();
                string[] rows = { "A", "B", "C", "D", "E", "F", "G", "H" };
                int seatId = 1;

                foreach (var movie in movies)
                {
                    for (int rowIdx = 0; rowIdx < rows.Length; rowIdx++)
                    {
                        int seatsInRow = rowIdx <= 1 ? 10 : (rowIdx <= 3 ? 12 : 14);
                        for (int i = 1; i <= seatsInRow; i++)
                        {
                            // Цены в белорусских рублях
                            int price;
                            if (rowIdx <= 1) // Ряды A, B - VIP
                                price = 15;
                            else if (rowIdx <= 3) // Ряды C, D, E - средние
                                price = 12;
                            else // Ряды F, G, H - обычные
                                price = 10;

                            await _dbContext.Seats.AddAsync(new Seat
                            {
                                Id = seatId++,
                                Row = rows[rowIdx],
                                Number = i,
                                IsAvailable = true,
                                Price = price,
                                MovieId = movie.Id
                            });
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
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
                SeatsPanel.Children.Clear();

                // Получаем забронированные места из новой системы (TicketRepository)
                var tickets = await _ticketRepository.GetAllAsync();
                var bookedSeats = tickets
                    .Where(t => t.MovieTitle == _selectedMovie?.Title)
                    .Select(t => t.SeatNumber)
                    .ToHashSet();

                var groupedSeats = seats.GroupBy(s => s.Row).OrderBy(g => g.Key);

                foreach (var group in groupedSeats)
                {
                    var rowLabel = new TextBlock
                    {
                        Text = $"Ряд {group.Key}",
                        FontWeight = FontWeights.Bold,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 14,
                        Margin = new Thickness(0, 15, 0, 8)
                    };
                    SeatsPanel.Children.Add(rowLabel);

                    var seatsWrapPanel = new WrapPanel { HorizontalAlignment = HorizontalAlignment.Center };

                    foreach (var seat in group.OrderBy(s => s.Number))
                    {
                        bool isBookedByNewSystem = bookedSeats.Contains(seat.SeatNumber);
                        bool isAvailable = seat.IsAvailable && !isBookedByNewSystem;

                        var button = new Button
                        {
                            Width = 50,
                            Height = 50,
                            Margin = new Thickness(4),
                            Style = (Style)FindResource("AnimatedSeatButton"),
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
                            Background = isAvailable
                                ? new SolidColorBrush(Color.FromRgb(76, 175, 80))
                                : new SolidColorBrush(Color.FromRgb(244, 67, 54)),
                            IsEnabled = isAvailable
                        };

                        button.Click += async (s, e) => await ShowSessionInfoPopup(seat);
                        seatsWrapPanel.Children.Add(button);
                    }

                    SeatsPanel.Children.Add(seatsWrapPanel);
                }

                var availableCount = seats.Count(s => s.IsAvailable && !bookedSeats.Contains(s.SeatNumber));
                StatusText.Text = $"Свободных мест: {availableCount} из {seats.Count}";
            }
            catch (Exception ex)
            {
                StatusText.Text = ($"Ошибка: {ex.Message}");
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private async Task ShowSessionInfoPopup(Seat seat)
        {
            if (_selectedMovie == null)
            {
                StatusText.Text = "Сначала выберите фильм";
                MessageBox.Show("Пожалуйста, выберите фильм из списка.", "Нет выбора фильма", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _popupOverlay = new Grid
            {
                Background = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var popupBorder = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(16),
                Padding = new Thickness(25),
                Width = 400,
                Height = 400,
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new ScaleTransform(0.7, 0.7),
                Opacity = 0,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 15,
                    ShadowDepth = 5,
                    Opacity = 0.3
                }
            };

            var content = new StackPanel();

            content.Children.Add(new TextBlock
            {
                Text = "Информация о сеансе",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 15),
                Foreground = new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            content.Children.Add(new Rectangle
            {
                Height = 2,
                Fill = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                Margin = new Thickness(0, 0, 0, 15)
            });

            content.Children.Add(new TextBlock
            {
                Text = $"Фильм: {_selectedMovie.Title}",
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 15),
                FontWeight = FontWeights.SemiBold
            });

            content.Children.Add(new TextBlock
            {
                Text = $"Время: {_selectedMovie.Time}",
                FontSize = 14,
                Margin = new Thickness(0, 5, 0, 0)
            });

            content.Children.Add(new TextBlock
            {
                Text = $"Длительность: {_selectedMovie.Duration} минут",
                FontSize = 14,
                Margin = new Thickness(0, 5, 0, 10)
            });

            content.Children.Add(new TextBlock
            {
                Text = $"Жанр: {_selectedMovie.Genre}",
                FontSize = 14,
                Margin = new Thickness(0, 5, 0, 0)
            });

            content.Children.Add(new TextBlock
            {
                Text = $"Зал: {_selectedMovie.HallNumber}",
                FontSize = 14,
                Margin = new Thickness(0, 0, 10, 0)
            });

            content.Children.Add(new Rectangle
            {
                Height = 1,
                Fill = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                Margin = new Thickness(0, 10, 0, 5)
            });

            content.Children.Add(new TextBlock
            {
                Text = $"Выбранное место: {seat.SeatNumber}",
                FontSize = 15,
                Margin = new Thickness(0, 5, 0, 10),
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(76, 175, 80))
            });

            content.Children.Add(new TextBlock
            {
                Text = $"Цена билета: {seat.Price} руб.",
                FontSize = 14,
                Margin = new Thickness(0, 5, 0, 15),
                Foreground = new SolidColorBrush(Color.FromRgb(255, 152, 0))
            });

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };

            var confirmButton = new Button
            {
                Content = "Подтвердить бронь",
                Width = 160,
                Height = 40,
                Margin = new Thickness(0, 0, 10, 0),
                Background = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                Foreground = Brushes.White,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Cursor = Cursors.Hand
            };
            confirmButton.Click += async (s, e) =>
            {
                await ClosePopup();
                await BookSeatAsync(seat);
            };

            var cancelButton = new Button
            {
                Content = "Отмена",
                Width = 120,
                Height = 40,
                Background = new SolidColorBrush(Color.FromRgb(158, 158, 158)),
                Foreground = Brushes.White,
                FontSize = 13,
                Cursor = Cursors.Hand
            };
            cancelButton.Click += async (s, e) => await ClosePopup();

            buttonPanel.Children.Add(confirmButton);
            buttonPanel.Children.Add(cancelButton);
            content.Children.Add(buttonPanel);

            popupBorder.Child = content;

            var grid = new Grid();
            grid.Children.Add(_popupOverlay);
            grid.Children.Add(popupBorder);

            popupBorder.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
            popupBorder.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);

            RootGrid.Children.Add(grid);
            grid.SetValue(Grid.RowSpanProperty, 3);
            Panel.SetZIndex(grid, 100);

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            var scaleX = new DoubleAnimation(0.7, 1, TimeSpan.FromSeconds(0.3));
            var scaleY = new DoubleAnimation(0.7, 1, TimeSpan.FromSeconds(0.3));
            popupBorder.BeginAnimation(OpacityProperty, fadeIn);
            popupBorder.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);
            popupBorder.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);
        }

        private async Task ClosePopup()
        {
            if (_popupOverlay?.Parent is Grid grid && grid.Children.Count > 0)
            {
                var popupBorder = grid.Children.OfType<Border>().FirstOrDefault();
                if (popupBorder != null)
                {
                    var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2));
                    var scaleOut = new DoubleAnimation(1, 0.9, TimeSpan.FromSeconds(0.2));
                    popupBorder.BeginAnimation(OpacityProperty, fadeOut);
                    popupBorder.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleOut);
                    popupBorder.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleOut);
                }
                await Task.Delay(200);
                RootGrid.Children.Remove(grid);
            }
            _popupOverlay = null;
        }

        private async Task BookSeatAsync(Seat seat)
        {
            if (_currentUser == null)
            {
                StatusText.Text = "Ошибка: пользователь не авторизован";
                return;
            }

            if (_selectedMovie == null)
            {
                StatusText.Text = "Сначала выберите фильм";
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
                var resultBooking = await _cinemaService.BookSeatAsync(booking);

                if (resultBooking != null)
                {
                    var session = (await _sessionRepository.GetAllAsync()).FirstOrDefault();
                    if (session != null)
                    {
                        var ticket = new Ticket
                        {
                            UserId = _currentUser.Id,
                            SessionId = session.Id,
                            CustomerName = _currentUser.DisplayName,
                            PhoneNumber = _currentUser.PhoneNumber ?? "",
                            SeatNumber = seat.SeatNumber,
                            Price = seat.Price,
                            MovieTitle = _selectedMovie.Title,
                            SessionTime = DateTime.Now,
                            BookingTime = DateTime.Now,
                            Status = "Confirmed"
                        };
                        await _ticketRepository.AddAsync(ticket);
                        await _ticketRepository.SaveChangesAsync();

                        await _cinemaViewModel.LoadTicketsAsync();
                    }

                    await LoadSeatsAsync(_selectedMovie.Id);
                    await LoadBookingsAsync();

                    StatusText.Text = "Билет успешно забронирован!";
                    MessageBox.Show($"Бронирование успешно!\n\nФильм: {_selectedMovie.Title}\nМесто: {seat.SeatNumber}\nСумма: {seat.Price} руб.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    StatusText.Text = "Место уже занято";
                    MessageBox.Show("Это место уже забронировано. Выберите другое место.", "Место занято", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Ошибка: {ex.Message}";
                MessageBox.Show($"Ошибка при бронировании: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                            var tickets = await _ticketRepository.GetAllAsync();
                            var ticketToDelete = tickets.FirstOrDefault(t => t.SeatNumber == selectedBooking.SeatNumber && t.MovieTitle == selectedBooking.MovieTitle);
                            if (ticketToDelete != null)
                            {
                                await _ticketRepository.DeleteAsync(ticketToDelete.Id);
                                await _ticketRepository.SaveChangesAsync();
                                await _cinemaViewModel.LoadTicketsAsync();
                            }

                            await LoadBookingsAsync();
                            if (_selectedMovie != null)
                            {
                                await LoadSeatsAsync(_selectedMovie.Id);
                            }

                            StatusText.Text = "Бронь отменена";
                            MessageBox.Show("Бронирование успешно отменено.", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show("Пожалуйста, выберите бронь из списка для отмены.", "Нет выбора", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void Logout(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await _authService.LogoutAsync();
                LoggedInUser = null;
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            }
        }
    }
}