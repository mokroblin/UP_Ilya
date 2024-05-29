using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UP_Ilya.Models;

namespace UP_Ilya.edit_windows
{
    public partial class EditCompany : Window
    {
        private readonly TV_ProgramContext _context;
        private readonly int _companyId;
        private readonly ObservableCollection<Company> _companies;

        public EditCompany(int companyId, ObservableCollection<Company> companies)
        {
            InitializeComponent();
            _context = new TV_ProgramContext();
            _companyId = companyId;
            _companies = companies;
            LoadCompany();
        }

        private void LoadCompany()
        {
            try
            {
                var company = _context.Companies.FirstOrDefault(c => c.CompanyID == _companyId);
                if (company != null)
                {
                    Company_Edit_CompanyNameTextBox.Text = company.CompanyName;
                    Compnay_Edit_CompanyPhoneTextBox.Text = company.CompanyPhone;
                }
                else
                {
                    MessageBox.Show("Компания не найдена.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных о компании: {ex.Message}");
                this.Close();
            }
        }

        private void EditCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var company = _context.Companies.FirstOrDefault(c => c.CompanyID == _companyId);
                if (company != null)
                {
                    company.CompanyName = Company_Edit_CompanyNameTextBox.Text;
                    company.CompanyPhone = Compnay_Edit_CompanyPhoneTextBox.Text;

                    _context.SaveChanges();

                    var companyInCollection = _companies.FirstOrDefault(c => c.CompanyID == _companyId);
                    if (companyInCollection != null)
                    {
                        companyInCollection.CompanyName = company.CompanyName;
                        companyInCollection.CompanyPhone = company.CompanyPhone;
                    }

                    MessageBox.Show("Редактирование прошло успешно.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Компания не найдена.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных о компании: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
