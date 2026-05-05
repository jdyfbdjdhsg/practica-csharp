using Day15.Models;
using Day15.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Day15.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ICinemaService _cinemaService;

        private ObservableCollection<Movie> _movies = new();
        private ObservableCollection<Seat> _seats = new();
        private ObservableCollection<Booking> _bookings = new();

        private Movie? _selectedMovie;
        private Booking? _selectedBooking;

        private bool _isLoadingMovies;
        private bool _isLoadingSeats;
        private bool _isBooking;
        private string _statusMessage = string.Empty;
        private int _loadingProgress;

        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set => SetProperty(ref _movies, value);
        }

        public ObservableCollection<Seat> Seats
        {
            get => _seats;
            set => SetProperty(ref _seats, value);
        }

        public ObservableCollection<Booking> Bookings
        {
            get => _bookings;
            set => SetProperty(ref _bookings, value);
        }

        public Movie? SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                if (SetProperty(ref _selectedMovie, value) && value != null)
                {
                    Task.Run(() => LoadSeatsAsync(value.Id));
                }
            }
        }

        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set => SetProperty(ref _selectedBooking, value);
        }

        public bool IsLoadingMovies
        {
            get => _isLoadingMovies;
            set => SetProperty(ref _isLoadingMovies, value);
        }

        public bool IsLoadingSeats
        {
            get => _isLoadingSeats;
            set => SetProperty(ref _isLoadingSeats, value);
        }

        public bool IsBooking
        {
            get => _isBooking;
            set => SetProperty(ref _isBooking, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public int LoadingProgress
        {
            get => _loadingProgress;
            set => SetProperty(ref _loadingProgress, value);
        }

        public IEnumerable<SeatGroup> SeatsGrouped
        {
            get
            {
                return Seats
                    .GroupBy(s => s.Row)
                    .OrderBy(g => g.Key)
                    .Select(g => new SeatGroup
                    {
                        Row = g.Key,
                        Seats = g.OrderBy(s => s.Number).ToList()
                    });
            }
        }

        public ICommand LoadMoviesCommand { get; }
        public ICommand BookSeatCommand { get; }
        public ICommand CancelBookingCommand { get; }
        public ICommand RefreshBookingsCommand { get; }

        public MainWindowViewModel()
        {
            _cinemaService = new CinemaService();

            LoadMoviesCommand = new RelayCommand(async _ => await LoadMoviesAsync());
            BookSeatCommand = new RelayCommand(param =>
            {
                var seat = param as Seat;
                if (seat != null && seat.IsAvailable)
                {
                    Task.Run(() => BookSeatAsync(seat));
                }
            }, param => param != null && !IsBooking);
            CancelBookingCommand = new RelayCommand(async _ => await CancelBookingAsync(), _ => SelectedBooking != null);
            RefreshBookingsCommand = new RelayCommand(async _ => await LoadBookingsAsync());

            Task.Run(() => LoadMoviesAsync());
        }

        private async Task LoadMoviesAsync()
        {
            IsLoadingMovies = true;
            StatusMessage = "Загрузка списка фильмов...";
            LoadingProgress = 0;

            try
            {
                for (int i = 0; i <= 100; i += 20)
                {
                    LoadingProgress = i;
                    await Task.Delay(300);
                }

                var movies = await _cinemaService.GetMoviesAsync();

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Movies.Clear();
                    foreach (var movie in movies)
                        Movies.Add(movie);
                });

                StatusMessage = $"Загружено {Movies.Count} фильмов";
                LoadingProgress = 100;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки: {ex.Message}";
            }
            finally
            {
                IsLoadingMovies = false;
                await Task.Delay(1000);
                StatusMessage = "Выберите фильм";
                LoadingProgress = 0;
            }
        }

        private async Task LoadSeatsAsync(int movieId)
        {
            IsLoadingSeats = true;
            StatusMessage = "Загрузка схемы зала...";

            try
            {
                var seats = await _cinemaService.GetSeatsAsync(movieId);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Seats.Clear();
                    foreach (var seat in seats)
                        Seats.Add(seat);

                    OnPropertyChanged(nameof(SeatsGrouped));
                });

                int availableCount = seats.Count(s => s.IsAvailable);
                StatusMessage = $"Свободных мест: {availableCount} из {seats.Count}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки мест: {ex.Message}";
            }
            finally
            {
                IsLoadingSeats = false;
            }
        }

        private async Task BookSeatAsync(Seat selectedSeat)
        {
            if (selectedSeat == null || SelectedMovie == null) return;

            IsBooking = true;
            StatusMessage = "Обработка бронирования...";

            string customerName = "Тестовый клиент";
            string phoneNumber = "123456789";

            var booking = new Booking
            {
                CustomerName = customerName,
                PhoneNumber = phoneNumber,
                MovieId = SelectedMovie.Id,
                MovieTitle = SelectedMovie.Title,
                SeatNumber = selectedSeat.SeatNumber,
                Price = selectedSeat.Price,
                BookingTime = DateTime.Now,
                IsConfirmed = false
            };

            try
            {
                await Task.Delay(2000);

                var result = await _cinemaService.BookSeatAsync(booking);

                if (result != null)
                {
                    selectedSeat.IsAvailable = false;
                    await LoadBookingsAsync();

                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        OnPropertyChanged(nameof(SeatsGrouped));
                    });

                    StatusMessage = $"Билет успешно забронирован для {customerName}!";

                    MessageBox.Show(
                        $"Бронирование успешно выполнено!\n\n" +
                        $"Клиент: {customerName}\n" +
                        $"Фильм: {SelectedMovie.Title}\n" +
                        $"Место: {selectedSeat.SeatNumber}\n" +
                        $"Сумма: {selectedSeat.Price} руб.",
                        "Успешно",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    StatusMessage = "Место уже занято";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
            finally
            {
                IsBooking = false;
                await Task.Delay(500);
                StatusMessage = "Готово";
            }
        }

        private async Task CancelBookingAsync()
        {
            if (SelectedBooking == null) return;

            var result = MessageBox.Show(
                $"Отменить бронь для {SelectedBooking.CustomerName}?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            StatusMessage = "Отмена бронирования...";

            try
            {
                var success = await _cinemaService.CancelBookingAsync(SelectedBooking.Id);

                if (success)
                {
                    await LoadSeatsAsync(SelectedBooking.MovieId);
                    await LoadBookingsAsync();

                    StatusMessage = "Бронь отменена";
                    MessageBox.Show("Бронирование отменено", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }

            await Task.Delay(500);
            StatusMessage = "Готово";
        }

        private async Task LoadBookingsAsync()
        {
            try
            {
                var bookings = await _cinemaService.GetBookingsAsync();

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Bookings.Clear();
                    foreach (var booking in bookings)
                        Bookings.Add(booking);
                });
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }
    }
}