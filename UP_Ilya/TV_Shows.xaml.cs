using ClosedXML.Excel;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using UP_Ilya.add_windows;
using UP_Ilya.edit_windows;
using UP_Ilya.Models;

namespace UP_Ilya
{
    public partial class ShowsWindow : Window
    {
        private readonly TV_ProgramContext _context;

        public ObservableCollection<TV_Show> TV_Shows { get; set; }

        public ShowsWindow()
        {
            InitializeComponent();
            _context = new TV_ProgramContext(); 
            LoadShows();
            Closing += ShowsWindow_Closing;
            WindowState = WindowState.Maximized;
        }

        private void LoadShows()
        {
            TV_Shows = new ObservableCollection<TV_Show>(_context.TV_Shows.ToList());
            ShowsDataGrid.ItemsSource = TV_Shows;
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

        private void ShowsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
        }

        private void AddTV_Show_Click(object sender, RoutedEventArgs e)
        {
            AddTV_Show addTV_Show = new AddTV_Show();
            addTV_Show.ShowDialog();
            RefreshTV_Shows(); 
        }

        private void EditTV_Show_Click(object sender, RoutedEventArgs e)
        {
            if (ShowsDataGrid.SelectedItem is TV_Show selectedTV_Show)
            {
                EditTV_Show editTV_ShowWindow = new EditTV_Show(selectedTV_Show.TVShowID, TV_Shows);
                editTV_ShowWindow.ShowDialog();

                
                RefreshTV_Shows();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите передачу для редактирования.");
            }
        }

        private void DeleteTV_Show_Click(object sender, RoutedEventArgs e)
        {
            DbUtility.DeleteItem(TV_Shows, ShowsDataGrid.SelectedItem, _context);
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

        private void TV_ShowsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Отчёт",
                DefaultExt = "xlsx",
                FileName = "TV_ShowsReport"
            };

            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("TV_Shows Report");

                worksheet.Cell(1, 1).Value = "TVShowID";
                worksheet.Cell(1, 2).Value = "TVShowName";
                worksheet.Cell(1, 3).Value = "AgeRating";
                worksheet.Cell(1, 4).Value = "PrimeTime";
                worksheet.Cell(1, 5).Value = "LiveDate";

                int rowIndex = 2;
                foreach (var tv_show in TV_Shows)
                {
                    worksheet.Cell(rowIndex, 1).Value = tv_show.TVShowID;
                    worksheet.Cell(rowIndex, 2).Value = tv_show.TVShowName;
                    worksheet.Cell(rowIndex, 3).Value = tv_show.AgeRating;
                    worksheet.Cell(rowIndex, 4).Value = tv_show.PrimeTime;
                    worksheet.Cell(rowIndex, 5).Value = tv_show.LiveDate;

                    rowIndex++;
                }

                var headerRange = worksheet.Range(1, 1, 1, 5);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.Yellow;

                workbook.SaveAs(saveFileDialog.FileName);
                MessageBox.Show("Отчёт таблицы Телепередачи успешно сохранён!");
            }
        }

    }
}
