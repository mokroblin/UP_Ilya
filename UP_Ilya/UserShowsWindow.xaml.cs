using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace UP_Ilya
{
    public partial class UserShowsWindow : Window
    {
        public UserShowsWindow()
        {
            InitializeComponent();
            LoadUserShows();
            Closing += UserShowsWindow_Closing;
            WindowState = WindowState.Maximized;
        }

        private void LoadUserShows()
        {
            string connectionString = "Server=DESKTOP-N7MIDVG\\SQLEXPRESS;Database=TV_Program;Integrated Security=True;";
            string query = "SELECT * FROM UserShows";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                UserShowsDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void AddTVShow_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=DESKTOP-N7MIDVG\\SQLEXPRESS;Database=TV_Program;Integrated Security=True;";
            string tvShowName = txtTVShowName.Text;
            string userName = txtUserName.Text;
            string userRole = txtUserRole.Text;

            if (string.IsNullOrEmpty(tvShowName) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userRole))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Найти ID пользователя по имени
                string findUserQuery = "SELECT UserID FROM Users WHERE UserName = @UserName";
                SqlCommand findUserCommand = new SqlCommand(findUserQuery, connection);
                findUserCommand.Parameters.AddWithValue("@UserName", userName);

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
                int tvShowId;

                if (tvShowIdObj == null)
                {
                    // Добавить новое шоу, если его нет
                    string insertTVShowQuery = "INSERT INTO TV_Show (TVShowName, AgeRating, PrimeTime, LiveDate) VALUES (@TVShowName, 'NR', '00:00:00', GETDATE()); SELECT SCOPE_IDENTITY();";
                    SqlCommand insertTVShowCommand = new SqlCommand(insertTVShowQuery, connection);
                    insertTVShowCommand.Parameters.AddWithValue("@TVShowName", tvShowName);

                    tvShowId = Convert.ToInt32(insertTVShowCommand.ExecuteScalar());
                }
                else
                {
                    tvShowId = (int)tvShowIdObj;
                }

                // Добавить запись в Watcher_TV_Show
                string insertWatcherTVShowQuery = "INSERT INTO Watcher_TV_Show (WatcherID, TVShowID) VALUES ((SELECT WatcherID FROM Watcher WHERE UserID = @UserID), @TVShowID)";
                SqlCommand insertWatcherTVShowCommand = new SqlCommand(insertWatcherTVShowQuery, connection);
                insertWatcherTVShowCommand.Parameters.AddWithValue("@UserID", userId);
                insertWatcherTVShowCommand.Parameters.AddWithValue("@TVShowID", tvShowId);

                insertWatcherTVShowCommand.ExecuteNonQuery();
            }

            // Обновить DataGrid
            LoadUserShows();

            // Очистить поля ввода
            txtTVShowName.Text = "";
            txtUserName.Text = "";
            txtUserRole.Text = "";
        }

        private void UserShowsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
        }

    }
}
