using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UP_Ilya.Models;

namespace UP_Ilya.add_windows
{
    public partial class AddTV_Show : Window
    {
        private readonly TV_ProgramContext _context;

        public AddTV_Show()
        {
            InitializeComponent();
            _context = new TV_ProgramContext();
        }

        private void AddTV_Show_Click(object sender, RoutedEventArgs e)
        {
            string tvshowname = TVShowNameTextBox.Text;
            string agerating = AgeRatingTextBox.Text;
            try
            {
                int companyid = int.Parse(CompanyIDTextBox.Text);
                DateOnly livedate = DateOnly.Parse(LiveDateTextBox.Text);
                TimeOnly primetime = TimeOnly.Parse(PrimeTimeTextBox.Text);

                TV_Show newTV_Show = new TV_Show


                {
                    CompanyID = companyid,
                    TVShowName = tvshowname,
                    AgeRating = agerating,
                    PrimeTime = primetime,
                    LiveDate = livedate,
                };

                _context.TV_Shows.Add(newTV_Show);
                _context.SaveChanges();

                System.Windows.MessageBox.Show("Шоу добавлено.");
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка добавления шоу в базу данных: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}