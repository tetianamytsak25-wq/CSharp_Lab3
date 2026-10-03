using System;
using System.Globalization;
using System.Windows;
using InterpolationApp.ViewModels;

namespace InterpolationApp
{
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            DataContext = viewModel;
        }

        private void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            string textH1 =H1TextBox.Text.Trim().Replace('.', ',');

            string textH2 =H2TextBox.Text.Trim().Replace('.', ',');

            bool validH1 = double.TryParse(textH1, NumberStyles.Float, CultureInfo.GetCultureInfo("uk-UA"), out double h1);
            bool validH2 = double.TryParse(textH2, NumberStyles.Float, CultureInfo.GetCultureInfo("uk-UA"), out double h2);

            if (!validH1 || !validH2)
            {
                MessageBox.Show("Крок 2: помилка введення чисел");
                return;
            }
            viewModel.Build(h1, h2);
        }
    }
}