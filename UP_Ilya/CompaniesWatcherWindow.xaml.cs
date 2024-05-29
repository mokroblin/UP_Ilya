using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UP_Ilya;
using UP_Ilya.Models;

namespace UP_Ilya
{
    public partial class CompaniesWatcherWindow : Window
    {
        private readonly TV_ProgramContext _context;

        public ObservableCollection<Company> Companies { get; set; }
        public CompaniesWatcherWindow()
        {
            InitializeComponent();
            _context = new TV_ProgramContext(); 
            LoadCompanies();
            Closing += CompaniesWatcherWindow_Closing;
            WindowState = WindowState.Maximized;
        }

        private void LoadCompanies()
        {
            Companies = new ObservableCollection<Company>(_context.Companies.ToList());
            CompaniesWatcherDataGrid.ItemsSource = Companies;
        }


        private void RefreshCompanies()
        {
            var companiesList = _context.Companies.ToList();
            Companies.Clear();
            foreach (var company in companiesList)
            {
                Companies.Add(company);
            }
        }

        private void CompaniesWatcherWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WatcherMenu watcherMenu = new WatcherMenu();
            watcherMenu.Show();
        }


        
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string watchersearchText = WatcherSearchTextBox.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(watchersearchText))
            {
                var filteredCompanies = _context.Companies
                    .Where(c => c.CompanyPhone.ToLower().Contains(watchersearchText))
                    .ToList();

                Companies.Clear();
                foreach (var company in filteredCompanies)
                {
                    Companies.Add(company);
                }

                // Проверка, если список фильтрованных передач пуст
                if (filteredCompanies.Count == 0)
                {
                    MessageBox.Show("Компании с указанным номером не найдены.", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            else
            {
                LoadCompanies();
            }
        }

        private void SearchButton2_Click(object sender, RoutedEventArgs e)
        {
            string watchersearchText = WatcherSearchTextBox.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(watchersearchText))
            {
                var filteredCompanies = _context.Companies
                    .Where(c => c.CompanyName.ToLower().Contains(watchersearchText))
                    .ToList();

                Companies.Clear();
                foreach (var company in filteredCompanies)
                {
                    Companies.Add(company);
                }

                // Проверка, если список фильтрованных передач пуст
                if (filteredCompanies.Count == 0)
                {
                    MessageBox.Show("Компании с указанным названием не найдены.", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            else
            {
                LoadCompanies();
            }
        }

        private void CompaniesWatcherWindowDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            WatcherSearchTextBox.Text = string.Empty;
            RefreshCompanies();
        }

    }
}
