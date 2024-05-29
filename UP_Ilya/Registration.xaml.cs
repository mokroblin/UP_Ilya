using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UP_Ilya.Models;

namespace UP_Ilya
{
    public partial class Registration : Window
    {
        private readonly TV_ProgramContext _context;

        public Registration()
        {
            InitializeComponent();
            _context = new TV_ProgramContext();

            WindowState = WindowState.Maximized;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Choose mainWindow = new Choose();
            mainWindow.Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string login = txtNewLogin.Text;
            string email = txtNewEmail.Text;
            string password = txtNewPassword.Password;
            string personalData = txtPersonalData.Text;

            if (IsPasswordValid(password))
            {
                try
                {
                    if (_context.Users.Any(u => u.UserName == login))
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Создаем нового пользователя
                    var newUser = new User
                    {
                        UserName = login,
                        UserMail = email,
                        UserPass = password,
                        UserRole = "Watcher" // Установим роль Watcher для новых пользователей
                    };
                    _context.Users.Add(newUser);
                    _context.SaveChanges();

                    // Создаем Watcher запись
                    var newWatcher = new Watcher
                    {
                        UserID = newUser.UserID,
                        PersonalData = personalData
                    };
                    _context.Watchers.Add(newWatcher);
                    _context.SaveChanges();

                    MessageBox.Show("Регистрация прошла успешно!");

                    // Переход к окну авторизации после успешной регистрации
                    Authorization authorizationWindow = new Authorization();
                    authorizationWindow.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пароль не соответствует требованиям безопасности.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (!IsPasswordValid(passwordBox.Password))
            {
                passwordBox.ToolTip = "Пароль должен содержать не менее 6 символов, включая как минимум одну прописную букву, одну цифру и один из символов: !@#$%^.";
                passwordBox.Background = new SolidColorBrush(Colors.LightCoral);
            }
            else
            {
                passwordBox.ToolTip = null;
                passwordBox.Background = new SolidColorBrush(Colors.White);
            }
        }

        private bool IsPasswordValid(string password)
        {
            // Пароль должен состоять из 6 символов с заглавными буквами, цифрами и спец-символами: !@#$%^
            return password.Length >= 6 &&
                   Regex.IsMatch(password, @"[A-Z]") &&
                   Regex.IsMatch(password, @"\d") &&
                   Regex.IsMatch(password, @"[!@#$%^]");
        }
    }
}
