using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Day18.Models;
using Day18.Services;

namespace Day18.ViewModels
{
    public class CinemaViewModel : BaseViewModel
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly User? _currentUser;

        public ObservableCollection<Session> Sessions { get; } = new();
        public ObservableCollection<Ticket> Tickets { get; } = new();

        private Session? _selectedSession;
        public Session? SelectedSession
        {
            get => _selectedSession;
            set
            {
                _selectedSession = value;
                OnPropertyChanged();
            }
        }

        private Ticket? _selectedTicket;
        public Ticket? SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                _selectedTicket = value;
                OnPropertyChanged();
            }
        }

        private string _selectedSeat = string.Empty;
        public string SelectedSeat
        {
            get => _selectedSeat;
            set
            {
                _selectedSeat = value;
                OnPropertyChanged();
            }
        }

        private string _customerName = string.Empty;
        public string CustomerName
        {
            get => _customerName;
            set
            {
                _customerName = value;
                OnPropertyChanged();
            }
        }

        private string _phoneNumber = string.Empty;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadSessionsCommand { get; }
        public ICommand LoadTicketsCommand { get; }
        public ICommand BookTicketCommand { get; }
        public ICommand CancelTicketCommand { get; }

        public CinemaViewModel(ISessionRepository sessionRepository, ITicketRepository ticketRepository, User? currentUser)
        {
            _sessionRepository = sessionRepository;
            _ticketRepository = ticketRepository;
            _currentUser = currentUser;

            if (_currentUser != null)
            {
                CustomerName = _currentUser.FullName;
                PhoneNumber = _currentUser.PhoneNumber;
            }

            LoadSessionsCommand = new AsyncRelayCommand(async () => await LoadSessionsAsync());
            LoadTicketsCommand = new AsyncRelayCommand(async () => await LoadTicketsAsync());
            BookTicketCommand = new AsyncRelayCommand(async () => await BookTicketAsync(), () => CanBookTicket());
            CancelTicketCommand = new AsyncRelayCommand(async () => await CancelTicketAsync(), () => SelectedTicket != null);
        }

        private bool CanBookTicket()
        {
            return SelectedSession != null &&
                   SelectedSession.AvailableSeats > 0 &&
                   !string.IsNullOrWhiteSpace(SelectedSeat) &&
                   !string.IsNullOrWhiteSpace(CustomerName);
        }

        private async Task LoadSessionsAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Загрузка сеансов...";

                var sessions = await _sessionRepository.GetAllAsync();

                Sessions.Clear();
                foreach (var session in sessions)
                {
                    Sessions.Add(session);
                }

                StatusMessage = $"Загружено {sessions.Count} сеансов";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadTicketsAsync()
        {
            try
            {
                if (_currentUser == null) return;

                var tickets = await _ticketRepository.GetByUserIdAsync(_currentUser.Id);

                Tickets.Clear();
                foreach (var ticket in tickets)
                {
                    Tickets.Add(ticket);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки билетов: {ex.Message}";
            }
        }

        private async Task BookTicketAsync()
        {
            if (SelectedSession == null || _currentUser == null)
            {
                StatusMessage = "Выберите сеанс";
                return;
            }

            if (SelectedSession.AvailableSeats <= 0)
            {
                StatusMessage = "Нет свободных мест на этот сеанс";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedSeat))
            {
                StatusMessage = "Введите номер места";
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = "Бронирование билета...";

                var ticket = new Ticket
                {
                    UserId = _currentUser.Id,
                    SessionId = SelectedSession.Id,
                    CustomerName = CustomerName,
                    PhoneNumber = PhoneNumber,
                    SeatNumber = SelectedSeat,
                    Price = SelectedSession.TicketPrice,
                    MovieTitle = SelectedSession.MovieTitle,
                    SessionTime = SelectedSession.StartTime
                };

                await _ticketRepository.AddAsync(ticket);
                await _ticketRepository.SaveChangesAsync();

                await LoadSessionsAsync();
                await LoadTicketsAsync();

                SelectedSeat = string.Empty;
                StatusMessage = $"Билет на место {ticket.SeatNumber} успешно забронирован!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка бронирования: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task CancelTicketAsync()
        {
            if (SelectedTicket == null)
            {
                StatusMessage = "Выберите билет для отмены";
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = "Отмена бронирования...";

                await _ticketRepository.DeleteAsync(SelectedTicket.Id);
                await _ticketRepository.SaveChangesAsync();

                await LoadSessionsAsync();
                await LoadTicketsAsync();

                StatusMessage = $"Билет на место {SelectedTicket.SeatNumber} отменен";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка отмены: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}