using System;
using System.Linq;
using System.Windows;
using UP_Ilya.Models;
using System.Text.RegularExpressions;

namespace UP_Ilya.add_windows
{
    public partial class AddCompany : Window
    {
        private readonly TV_ProgramContext _context;

        public AddCompany()
        {
            InitializeComponent();
            _context = new TV_ProgramContext();
        }

        private void AddCompany_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve data from text boxes
            string companyname = CompanyNameTextBox.Text;
            string companyphone = CompanyPhoneTextBox.Text;

            // Validate input data
            if (string.IsNullOrWhiteSpace(companyname))
            {
                MessageBox.Show("Название компании не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(companyphone))
            {
                MessageBox.Show("Телефон компании не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if company name contains digits
            if (Regex.IsMatch(companyname, @"\d"))
            {
                MessageBox.Show("Название компании не может содержать цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create new consumer instance
            Company newCompany = new Company
            {
                CompanyName = companyname,
                CompanyPhone = companyphone,
            };

            try
            {
                // Add to database
                _context.Companies.Add(newCompany);
                _context.SaveChanges();

                // Inform user and close the window
                MessageBox.Show("Компания добавлена.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления компании в базу данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
