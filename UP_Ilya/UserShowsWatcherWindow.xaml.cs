using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace UP_Ilya
{
    public partial class UserShowsWatcherWindow : Window
    {
        private readonly string _userName;

        public UserShowsWatcherWindow(string userName)
        {
            InitializeComponent();
            _userName = userName;
            LoadUserShows(_userName);
            LoadTVShowNames(); //Загрузка имён в комбобокс
            Closing += UserShowsWindow_Closing;
            WindowState = WindowState.Maximized;
        }

        private void LoadUserShows(string userName)
        {
            string connectionString = "Server=DESKTOP-N7MIDVG\\SQLEXPRESS;Database=TV_Program;Integrated Security=True;";
            string query = "SELECT * FROM UserShows WHERE UserName = @UserName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@UserName", userName);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                UserShowsDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void LoadTVShowNames()
        {
            string connectionString = "Server=DESKTOP-N7MIDVG\\SQLEXPRESS;Database=TV_Program;Integrated Security=True;";
            string query = "SELECT TVShowName FROM TV_Show";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cmbTVShowName.Items.Add(reader["TVShowName"].ToString());
                }

                reader.Close();
            }
        }

        private void AddTVShow_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=DESKTOP-N7MIDVG\\SQLEXPRESS;Database=TV_Program;Integrated Security=True;";
            string tvShowName = cmbTVShowName.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(tvShowName))
            {
                MessageBox.Show("Пожалуйста, выберите передачу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Найти ID пользователя по имени
                string findUserQuery = "SELECT UserID FROM Users WHERE UserName = @UserName";
                SqlCommand findUserCommand = new SqlCommand(findUserQuery, connection);
                findUserCommand.Parameters.AddWithValue("@UserName", _userName);

                object userIdObj = findUserCommand.ExecuteScalar();
                if (userIdObj == null)
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int userId = (int)userIdObj;

                // Найти ID шоу по названию
                string findTVShowQuery = "SELECT TVShowID FROM TV_Show WHERE TVShowName = @TVShowName";
                SqlCommand findTVShowCommand = new SqlCommand(findTVShowQuery, connection);
                findTVShowCommand.Parameters.AddWithValue("@TVShowName", tvShowName);

                object tvShowIdObj = findTVShowCommand.ExecuteScalar();
                if (tvShowIdObj == null)
                {
                    MessageBox.Show("Данной передачи не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int tvShowId = (int)tvShowIdObj;

                // Проверить, существует ли запись в Watcher_TV_Show
                string checkWatcherTVShowQuery = "SELECT COUNT(*) FROM Watcher_TV_Show WHERE WatcherID = (SELECT WatcherID FROM Watcher WHERE UserID = @UserID) AND TVShowID = @TVShowID";
                SqlCommand checkWatcherTVShowCommand = new SqlCommand(checkWatcherTVShowQuery, connection);
                checkWatcherTVShowCommand.Parameters.AddWithValue("@UserID", userId);
                checkWatcherTVShowCommand.Parameters.AddWithValue("@TVShowID", tvShowId);

                int count = (int)checkWatcherTVShowCommand.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Эта передача уже добавлена для пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Добавить запись в Watcher_TV_Show
                string insertWatcherTVShowQuery = "INSERT INTO Watcher_TV_Show (WatcherID, TVShowID) VALUES ((SELECT WatcherID FROM Watcher WHERE UserID = @UserID), @TVShowID)";
                SqlCommand insertWatcherTVShowCommand = new SqlCommand(insertWatcherTVShowQuery, connection);
                insertWatcherTVShowCommand.Parameters.AddWithValue("@UserID", userId);
                insertWatcherTVShowCommand.Parameters.AddWithValue("@TVShowID", tvShowId);

                insertWatcherTVShowCommand.ExecuteNonQuery();
            }

            // Обновить DataGrid
            LoadUserShows(_userName);

            // Сбросить выбор в ComboBox
            cmbTVShowName.SelectedItem = null;
        }

        private void UserShowsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WatcherMenu watcherMenu = new WatcherMenu();
            watcherMenu.Show();
        }
    }
}
