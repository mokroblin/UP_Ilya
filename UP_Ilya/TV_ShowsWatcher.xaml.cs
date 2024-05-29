using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using UP_Ilya;
using UP_Ilya.Models;

namespace UP_Ilya
{
    public partial class ShowsWatcherWindow : Window
    {
        private readonly TV_ProgramContext _context;

        public ObservableCollection<TV_Show> TV_Shows { get; set; }
        public ShowsWatcherWindow()
        {
            InitializeComponent();
            _context = new TV_ProgramContext(); 
            LoadShows();
            Closing += ShowsWatcherWindow_Closing;
            WindowState = WindowState.Maximized;
        }

        private void LoadShows()
        {
            TV_Shows = new ObservableCollection<TV_Show>(_context.TV_Shows.ToList());
            ShowsWatcherDataGrid.ItemsSource = TV_Shows;
        }

        private void RefreshTV_Shows()
        {
            var tv_showsList = _context.TV_Shows.ToList();
            TV_Shows.Clear();
            foreach (var tv_show in tv_showsList)
            {
                TV_Shows.Add(tv_show);
            }
        }

        private void ShowsWatcherWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WatcherMenu watcherMenu = new WatcherMenu();
            watcherMenu.Show();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();
            DateOnly searchDate = default;  // Инициализация значением по умолчанию
            TimeOnly searchTime = default;  // Инициализация значением по умолчанию
            bool isDateSearch = false;
            bool isTimeSearch = false;

            // Проверка формата даты ДД-ММ-ГГГГ
            if (Regex.IsMatch(searchText, @"^\d{2}-\d{2}-\d{4}$"))
            {
                DateTime parsedDate;
                if (DateTime.TryParseExact(searchText, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    searchDate = DateOnly.FromDateTime(parsedDate);
                    isDateSearch = true;
                }
            }

            // Проверка формата времени
            isTimeSearch = TimeOnly.TryParse(searchText, out searchTime);

            if (!string.IsNullOrEmpty(searchText))
            {
                var filteredTV_Shows = _context.TV_Shows.Where(tv_show =>
                    (isDateSearch && tv_show.LiveDate == searchDate) ||
                    (isTimeSearch && tv_show.PrimeTime.ToString("HH:mm") == searchTime.ToString("HH:mm")) || // Форматирование времени
                    tv_show.TVShowName.ToLower().Contains(searchText))
                    .ToList();

                TV_Shows.Clear();
                foreach (var tv_show in filteredTV_Shows)
                {
                    TV_Shows.Add(tv_show);
                }

                // Проверка, если список фильтрованных передач пуст
                if (filteredTV_Shows.Count == 0)
                {
                    MessageBox.Show("Передачи с указанной датой не найдены.", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                LoadShows();
            }
        }


        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            RefreshTV_Shows();
        }

        private void TV_ShowsWatcherDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
