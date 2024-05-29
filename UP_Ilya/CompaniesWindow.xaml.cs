using ClosedXML.Excel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using UP_Ilya.add_windows;
using UP_Ilya.edit_windows;
using UP_Ilya.Models;

namespace UP_Ilya
{
    public partial class CompaniesWindow : Window
    {
        private readonly TV_ProgramContext _context;

        public ObservableCollection<Company> Companies { get; set; }
        public CompaniesWindow()
        {
            InitializeComponent();
            _context = new TV_ProgramContext(); 
            LoadCompanies();
            Closing += CompaniesWindow_Closing;
            WindowState = WindowState.Maximized;
        }

        private void LoadCompanies()
        {
            Companies = new ObservableCollection<Company>(_context.Companies.ToList());
            CompaniesDataGrid.ItemsSource = Companies;
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
        private void CompaniesWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
        }

        private void AddCompany_Click(object sender, RoutedEventArgs e)
        {
            AddCompany addCompany = new AddCompany();
            addCompany.ShowDialog();
            RefreshCompanies(); 
        }

        private void EditCompany_Click(object sender, RoutedEventArgs e)
        {
            if (CompaniesDataGrid.SelectedItem is Company selectedCompany)
            {
                EditCompany editCompany = new EditCompany(selectedCompany.CompanyID, Companies);
                editCompany.ShowDialog();

                
                RefreshCompanies();
            }
            else
            {
                MessageBox.Show("Выберите строку.");
            }
        }

        private void DeleteCompany_Click(object sender, RoutedEventArgs e)
        {
            DbUtility.DeleteItem(Companies, CompaniesDataGrid.SelectedItem, _context);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(searchText))
            {
                var filteredCompanies = _context.Companies
                    .Where(c => c.CompanyPhone.ToLower().Contains(searchText))
                    .ToList();

                Companies.Clear();
                foreach (var company in filteredCompanies)
                {
                    Companies.Add(company);
                }

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
            string searchText = SearchTextBox.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(searchText))
            {
                var filteredCompanies = _context.Companies
                    .Where(c => c.CompanyName.ToLower().Contains(searchText))
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

        private void CompaniesWindowDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            RefreshCompanies();
        }

        

    }
}
