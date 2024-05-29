using System.Linq;
using System.Windows;
using UP_Ilya.Models;

namespace UP_Ilya
{
    public partial class Authorization : Window
    {
        private readonly TV_ProgramContext _context;

        public Authorization()
        {
            InitializeComponent();
            _context = new TV_ProgramContext();
            WindowState = WindowState.Maximized;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var login = txtLogin.Text;
            var password = txtPassword.Password;

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == login && u.UserPass == password);
                if (user != null)
                {
                    MessageBox.Show("Добро пожаловать.");
                    CurrentUser.UserName = login; // Сохраняем имя пользователя
                    if (login == "mokroblin")
                    {
                        AdminMenu adminmenu = new AdminMenu();
                        adminmenu.Show();
                        this.Close();
                    }
                    else
                    {
                        WatcherMenu watcherMenu = new WatcherMenu();
                        watcherMenu.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль. Попробуйте снова.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении к базе данных: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Choose choose = new Choose();
            choose.Show();
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
