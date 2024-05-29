using System.Windows;

namespace UP_Ilya
{
    public partial class WatcherMenu : Window
    {
        public WatcherMenu()
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

        private void WatcherCompaniesButton_Click(object sender, RoutedEventArgs e)
        {
            CompaniesWatcherWindow watchercompaniesWindow = new CompaniesWatcherWindow();
            watchercompaniesWindow.Show();
            this.Close();
        }

        private void TV_ShowsWatcherButton_Click(object sender, RoutedEventArgs e)
        {
            ShowsWatcherWindow showswatcherWindow = new ShowsWatcherWindow();
            showswatcherWindow.Show();
            this.Close();
        }

        private void UserShowButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение имени пользователя из глобальной переменной
            string currentUserName = CurrentUser.UserName;

            UserShowsWatcherWindow userWatcherShows = new UserShowsWatcherWindow(currentUserName);
            userWatcherShows.Show();
            this.Close();
        }
    }
}
