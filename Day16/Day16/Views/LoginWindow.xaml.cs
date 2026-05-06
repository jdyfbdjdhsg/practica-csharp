using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Day16.Services;

namespace Day16.Views
{
    public partial class LoginWindow : Window
    {
        private readonly IAuthService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthService();

            LoginButton.Click += async (s, e) => await LoginAsync();
            ShowRegisterButton.Click += (s, e) => ShowRegisterPanel();
            RegisterButton.Click += async (s, e) => await RegisterAsync();
            BackToLoginButton.Click += (s, e) => ShowLoginPanel();
        }

        private void ShowLoginPanel()
        {
            LoginPanel.Visibility = Visibility.Visible;
            RegisterPanel.Visibility = Visibility.Collapsed;
            ErrorText.Visibility = Visibility.Collapsed;
            RegErrorText.Visibility = Visibility.Collapsed;
        }

        private void ShowRegisterPanel()
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;
            ErrorText.Visibility = Visibility.Collapsed;
            RegErrorText.Visibility = Visibility.Collapsed;
            RegUsernameBox.Text = "";
            RegFullNameBox.Text = "";
            RegPhoneBox.Text = "";
            RegPasswordBox.Password = "";
            RegConfirmPasswordBox.Password = "";
        }

        private async Task LoginAsync()
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Введите логин");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Введите пароль");
                return;
            }

            ProgressBar.Visibility = Visibility.Visible;
            LoginButton.IsEnabled = false;
            ErrorText.Visibility = Visibility.Collapsed;

            try
            {
                var user = await _authService.LoginAsync(username, password);

                if (user != null)
                {
                    // Передаем пользователя в главное окно через статическое свойство
                    MainWindow.LoggedInUser = user;

                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ShowError("Неверный логин или пароль");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }
            finally
            {
                ProgressBar.Visibility = Visibility.Collapsed;
                LoginButton.IsEnabled = true;
            }
        }

        private async Task RegisterAsync()
        {
            string username = RegUsernameBox.Text.Trim();
            string fullName = RegFullNameBox.Text.Trim();
            string phone = RegPhoneBox.Text.Trim();
            string password = RegPasswordBox.Password;
            string confirmPassword = RegConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            {
                ShowRegError("Логин должен содержать минимум 3 символа");
                return;
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ShowRegError("Введите ФИО");
                return;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            {
                ShowRegError("Пароль должен содержать минимум 4 символа");
                return;
            }

            if (password != confirmPassword)
            {
                ShowRegError("Пароли не совпадают");
                return;
            }

            RegProgressBar.Visibility = Visibility.Visible;
            RegisterButton.IsEnabled = false;
            RegErrorText.Visibility = Visibility.Collapsed;

            try
            {
                var success = await _authService.RegisterAsync(username, password, fullName, phone);

                if (success)
                {
                    MessageBox.Show("Регистрация успешна! Теперь вы можете войти.", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowLoginPanel();
                    UsernameBox.Text = username;
                    PasswordBox.Password = "";
                }
                else
                {
                    ShowRegError("Пользователь с таким логином уже существует");
                }
            }
            catch (Exception ex)
            {
                ShowRegError($"Ошибка: {ex.Message}");
            }
            finally
            {
                RegProgressBar.Visibility = Visibility.Collapsed;
                RegisterButton.IsEnabled = true;
            }
        }

        private void ShowError(string message)
        {
            ErrorText.Text = message;
            ErrorText.Visibility = Visibility.Visible;
        }

        private void ShowRegError(string message)
        {
            RegErrorText.Text = message;
            RegErrorText.Visibility = Visibility.Visible;
        }
    }
}