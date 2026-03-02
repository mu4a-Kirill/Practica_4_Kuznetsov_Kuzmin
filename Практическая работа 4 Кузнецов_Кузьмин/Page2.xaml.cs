using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Практическая_работа_4_Кузнецов_Кузьмин
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
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
                double x = double.Parse(XTextBox.Text);
                double b = double.Parse(BTextBox.Text);

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
                MessageBox.Show("Пожалуйста, введите корректные числа.", "Ошибка ввода");
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
