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
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double x = double.Parse(XTextBox.Text);
                double y = double.Parse(YTextBox.Text);
                double z = double.Parse(ZTextBox.Text);

                if (z < -1 || z > 1)
                {
                    MessageBox.Show("Значение Z должно быть в диапазоне [-1, 1] для арксинуса.", "Ошибка");
                    return;
                }

                double cubeRootX = Math.Pow(x, 1.0 / 3.0);
                double xPowerYPlus2 = Math.Pow(x, y + 2);
                double arcsinZ = Math.Asin(z);
                double arcsinZSquare = arcsinZ * arcsinZ;
                double absXY = Math.Abs(x - y);
                double underSqrt = 10 * (cubeRootX + xPowerYPlus2) * (arcsinZSquare - absXY);

                if (underSqrt < 0)
                {
                    MessageBox.Show("Подкоренное выражение не может быть отрицательным.", "Ошибка");
                    return;
                }

                double beta = Math.Sqrt(underSqrt);
                ResultTextBox.Text = beta.ToString("F3");
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
            YTextBox.Clear();
            ZTextBox.Clear();
            ResultTextBox.Clear();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }

        private void YTextBox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            YTextBox.ToolTip = GetYToolTip();
        }

        private void ZTextBox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ZTextBox.ToolTip = GetZToolTip();
        }

        private string GetYToolTip()
        {
            double x, z;
            if (!double.TryParse(XTextBox.Text, out x) || !double.TryParse(ZTextBox.Text, out z))
                return "Сначала введите корректные значения X и Z.";

            if (z < -1 || z > 1)
                return "Значение Z должно быть от -1 до 1 для арксинуса.";

            if (x <= 0)
                return "Для корректного вычисления X должно быть положительным.";

            double arcsinZ = Math.Asin(z);
            double arcsinZ2 = arcsinZ * arcsinZ;
            double left = x - arcsinZ2;
            double right = x + arcsinZ2;

            return $"Рекомендуемый диапазон Y: от {left:F3} до {right:F3} (при текущих X и Z).";
        }

        private string GetZToolTip()
        {
            double x, y;
            if (!double.TryParse(XTextBox.Text, out x) || !double.TryParse(YTextBox.Text, out y))
                return "Сначала введите корректные значения X и Y.";

            double diff = Math.Abs(x - y);
            double maxArcsin2 = Math.Pow(Math.PI / 2, 2); // (π/2)^2 ≈ 2.467

            if (diff > maxArcsin2)
                return "Невозможно подобрать Z, так как |x-y| слишком велико. Уменьшите |x-y|.";

            double sqrtDiff = Math.Sqrt(diff);
            double sinVal = Math.Sin(sqrtDiff);
            double leftBound = -sinVal;
            double rightBound = sinVal;

            return $"Рекомендуемый диапазон Z: от -1 до {leftBound:F3} или от {rightBound:F3} до 1.";
        }
    }
}