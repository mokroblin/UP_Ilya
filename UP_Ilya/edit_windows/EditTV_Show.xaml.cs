using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using UP_Ilya.Models;

namespace UP_Ilya.edit_windows
{
    public partial class EditTV_Show : Window
    {
        private readonly TV_ProgramContext _context;
        private readonly int _tvshowId;
        private readonly ObservableCollection<TV_Show> _tv_shows;

        public EditTV_Show(int tv_showId, ObservableCollection<TV_Show> tv_shows)
        {
            InitializeComponent();
            _context = new TV_ProgramContext();
            _tvshowId = tv_showId; // Correct assignment
            _tv_shows = tv_shows;
            LoadTV_ShowData();
        }

        private void LoadTV_ShowData()
        {
            try
            {
                // Retrieve the TV show from the database using the provided tvshowId
                var tv_show = _context.TV_Shows.FirstOrDefault(p => p.TVShowID == _tvshowId);
                if (tv_show != null)
                {
                    // Populate the fields with existing data
                    TV_Show_Edit_CompanyIDTextBox.Text = tv_show.CompanyID.ToString();
                    TV_Show_Edit_TVShowNameTextBox.Text = tv_show.TVShowName;
                    TV_Show_Edit_AgeRatingTextBox.Text = tv_show.AgeRating;
                    TV_Show_Edit_PrimeTimeTextBox.Text = tv_show.PrimeTime.ToString();
                    TV_Show_Edit_LiveDateTextBox.Text = tv_show.LiveDate.ToString("dd-MM-yyyy");
                }
                else
                {
                    MessageBox.Show("Шоу не найдено.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных о шоу: {ex.Message}");
                this.Close();
            }
        }

        private void EditTV_Show_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve the TV show from the database using the provided tvshowId
                var tv_show = _context.TV_Shows.FirstOrDefault(p => p.TVShowID == _tvshowId);
                if (tv_show != null)
                {
                    // Update the TV show properties with the new values from the UI
                    tv_show.TVShowName = TV_Show_Edit_TVShowNameTextBox.Text;

                    // Parse the delivery date string to DateOnly
                    DateOnly liveDate;
                    if (DateOnly.TryParse(TV_Show_Edit_LiveDateTextBox.Text, out liveDate))
                    {
                        // Assign the parsed DateOnly to the LiveDate property
                        tv_show.LiveDate = liveDate;
                    }
                    else
                    {
                        MessageBox.Show("Неверный формат даты.");
                        return;
                    }

                    // Parse the prime time string to TimeOnly
                    TimeOnly primeTime;
                    if (TimeOnly.TryParse(TV_Show_Edit_PrimeTimeTextBox.Text, out primeTime))
                    {
                        // Assign the parsed TimeOnly to the PrimeTime property
                        tv_show.PrimeTime = primeTime;
                    }
                    else
                    {
                        MessageBox.Show("Неверный формат времени.");
                        return;
                    }

                    tv_show.CompanyID = int.Parse(TV_Show_Edit_CompanyIDTextBox.Text);

                    // Save changes to the database
                    _context.SaveChanges();

                    // Update the observable collection
                    var tv_showInCollection = _tv_shows.FirstOrDefault(p => p.TVShowID == _tvshowId);
                    if (tv_showInCollection != null)
                    {
                        tv_showInCollection.CompanyID = tv_show.CompanyID;
                        tv_showInCollection.TVShowName = tv_show.TVShowName;
                        tv_showInCollection.AgeRating = tv_show.AgeRating;
                        tv_showInCollection.PrimeTime = tv_show.PrimeTime;
                        tv_showInCollection.LiveDate = tv_show.LiveDate;
                    }

                    MessageBox.Show("Данные шоу обновлены.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Шоу не найдено.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка редактирования данных шоу: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Close the window without making any changes
            this.Close();
        }
    }
}
