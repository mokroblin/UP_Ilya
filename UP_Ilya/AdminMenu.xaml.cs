using System.Windows;
using UP_Ilya.Models; // Добавлен импорт пространства имен, где определен тип User

namespace UP_Ilya
{
    public partial class AdminMenu : Window
    {
        public AdminMenu()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            UserOptions.Visibility = UserOptions.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ChangeAccountButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите сменить аккаунт?", "Подтверждение смены аккаунта", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Смена аккаунта");
                Authorization authorizationWindow = new Authorization();
                authorizationWindow.Show();
                this.Close();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из системы?", "Подтверждение выхода", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Выход из системы");
                Choose choosing = new Choose();
                choosing.Show();
                this.Close();
            }
        }
        private void CompaniesButton_Click(object sender, RoutedEventArgs e)
        {
            CompaniesWindow companiesWindow = new CompaniesWindow();
            companiesWindow.Show();
            this.Close();
        }

        private void TV_ShowsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowsWindow showsWindow = new ShowsWindow();
            showsWindow.Show();
            this.Close();
        }
        //возможность смены пароля

        private void UserAccountButton_Click(object sender, RoutedEventArgs e)
        {
            UserAccounts userAccounts = new UserAccounts();
            userAccounts.Show();
            this.Close();
        }

        private void UserShowButton_Click(object sender, RoutedEventArgs e)
        {
            UserShowsWindow userShows = new UserShowsWindow();
            userShows.Show();
            this.Close();
        }

    }
}