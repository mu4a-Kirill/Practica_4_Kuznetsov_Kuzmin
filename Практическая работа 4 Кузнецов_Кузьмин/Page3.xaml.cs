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
using OxyPlot;
using OxyPlot.Series;

namespace Практическая_работа_4_Кузнецов_Кузьмин
{
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            // Создаём модель графика и задаём заголовок
            var plotModel = new PlotModel { Title = "График функции y = x² + tan(5x + d/x)" };
            // Настраиваем оси для автоматического масштабирования
            plotModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, Title = "x" });
            plotModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Left, Title = "y" });
            FunctionPlot.Model = plotModel;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double x0 = double.Parse(X0TextBox.Text);
                double xk = double.Parse(XkTextBox.Text);
                double dx = double.Parse(DxTextBox.Text);
                double d = double.Parse(DTextBox.Text);

                if (xk <= x0)
                {
                    MessageBox.Show("Xk должно быть больше X0.", "Ошибка");
                    return;
                }
                if (dx <= 0)
                {
                    MessageBox.Show("Шаг Dx должен быть положительным.", "Ошибка");
                    return;
                }

                // Очищаем предыдущие данные
                ResultTextBox.Clear();
                var plotModel = FunctionPlot.Model;
                plotModel.Series.Clear();

                var series = new LineSeries
                {
                    Title = "y = x² + tan(5x + d/x)",
                    Color = OxyColors.Blue
                };

                bool hasErrors = false;
                int pointsAdded = 0;

                for (double x = x0; x <= xk; x += dx)
                {
                    // Проверка деления на ноль в аргументе тангенса (x=0)
                    if (Math.Abs(x) < 1e-9)
                    {
                        ResultTextBox.AppendText($"x = {x:F3}: деление на ноль (x = 0) невозможно\r\n");
                        hasErrors = true;
                        continue;
                    }

                    double argument = 5 * x + d / x;
                    double tanValue;

                    // Пытаемся вычислить тангенс. В C# Math.Tan не выбрасывает исключение, 
                    // но может вернуть очень большое число или бесконечность вблизи асимптот.
                    tanValue = Math.Tan(argument);

                    // Проверяем, не является ли результат бесконечным или нечисловым значением
                    if (double.IsInfinity(tanValue) || double.IsNaN(tanValue))
                    {
                        ResultTextBox.AppendText($"x = {x:F3}: тангенс не определён (близко к асимптоте)\r\n");
                        hasErrors = true;
                        continue;
                    }

                    double y = x * x + tanValue;

                    // Дополнительная проверка на слишком большие значения (чтобы график не сжимался)
                    if (Math.Abs(y) > 1e6)
                    {
                        ResultTextBox.AppendText($"x = {x:F3}: значение y = {y:F0} слишком велико для отображения\r\n");
                        hasErrors = true;
                        continue;
                    }

                    series.Points.Add(new DataPoint(x, y));
                    pointsAdded++;
                    ResultTextBox.AppendText($"x = {x:F3}\t y = {y:F3}\r\n");
                }

                if (pointsAdded > 0)
                {
                    plotModel.Series.Add(series);
                }
                else
                {
                    ResultTextBox.AppendText("Нет точек для построения графика.\r\n");
                }

                // Обновляем график
                plotModel.InvalidatePlot(true);

                if (hasErrors)
                {
                    ResultTextBox.AppendText("Некоторые точки были пропущены.\r\n");
                }
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
            // Возвращаем базовые значения
            X0TextBox.Text = "0.1";
            XkTextBox.Text = "5";
            DxTextBox.Text = "0.2";
            DTextBox.Text = "1";
            ResultTextBox.Clear();
            var plotModel = FunctionPlot.Model;
            plotModel.Series.Clear();
            plotModel.InvalidatePlot(true);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }
    }
}