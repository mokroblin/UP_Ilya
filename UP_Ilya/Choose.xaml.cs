using System.Net;
using System.Windows;
using UP_Ilya;

namespace UP_Ilya
{
    public partial class Choose : Window
    {
        public Choose()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void Authorize_Click(object sender, RoutedEventArgs e)
        {
            Authorization authWindow = new Authorization();
            authWindow.Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Registration regWindow = new Registration();
            regWindow.Show();
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}