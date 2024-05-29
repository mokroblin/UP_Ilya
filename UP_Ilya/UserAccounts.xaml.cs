using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using UP_Ilya;
using UP_Ilya.Models;

namespace UP_Ilya
{
    public partial class UserAccounts : Window
    {
        private readonly TV_ProgramContext _context;

        public ObservableCollection<User> Users { get; set; }

        public UserAccounts()
        {
            InitializeComponent();
            _context = new TV_ProgramContext(); // Инициалиция контекста
            LoadUserAccounts();
            Closing += UserAccounts_Closing;
            WindowState = WindowState.Maximized;
        }
        private void UserAccounts_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
        }

        private void LoadUserAccounts()
        {
            Users = new ObservableCollection<User>(_context.Users.ToList());
            UserAccountsDataGrid.ItemsSource = Users;
        }

        private void RefreshUserAccounts()
        {
            var useraccountsList = _context.Users.ToList();
            Users.Clear();
            foreach (var user in useraccountsList)
            {
                Users.Add(user);
            }
        }

        private void UserAccountsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            RefreshUserAccounts();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(searchText))
            {
                var filteredUserAccounts = _context.Users
                    .Where(c => c.UserName.ToLower().Contains(searchText))
                    .ToList();

                Users.Clear();
                foreach (var user in filteredUserAccounts)
                {
                    Users.Add(user);
                }
            }
            else
            {
                LoadUserAccounts();
            }
        }

    }
}