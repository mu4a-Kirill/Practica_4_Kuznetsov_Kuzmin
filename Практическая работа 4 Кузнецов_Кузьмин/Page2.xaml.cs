using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

namespace Практическая_работа_4_Кузнецов_Кузьмин
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private double ParseDouble(string text)
        {
            string s = text.Replace(',', '.');
            return double.Parse(s, CultureInfo.InvariantCulture);
        }

        private double GetF(double x)
        {
            if (ShRadio.IsChecked == true)
                return Math.Sinh(x);
            else if (SquareRadio.IsChecked == true)
                return x * x;
            else
                return Math.Exp(x);
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double x = ParseDouble(XTextBox.Text);
                double b = ParseDouble(BTextBox.Text);

                double f = GetF(x);
                double xb = x * b;

                double g;

                if (xb > 0.5 && xb < 10)
                {
                    g = Math.Exp(f - Math.Abs(b));
                }
                else if (xb > 0.1 && xb < 0.5)
                {
                    g = Math.Sqrt(Math.Abs(f) + Math.Abs(b));
                }
                else
                {
                    g = 2 * f * f;
                }

                ResultTextBox.Text = g.ToString("F3");
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите корректные числа (используйте . или ,).", "Ошибка ввода");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            XTextBox.Clear();
            BTextBox.Clear();
            ResultTextBox.Clear();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page3());
        }
    }
}