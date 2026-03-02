using System;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Series;
using System.Globalization;

namespace Практическая_работа_4_Кузнецов_Кузьмин
{
    public partial class Page3 : Page
    {
        private PlotModel _plotModel;

        public Page3()
        {
            InitializeComponent();
            _plotModel = new PlotModel();
            _plotModel.Title = "График функции y = x² + tan(5x + d/x)";
            _plotModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, Title = "x" });
            _plotModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Left, Title = "y" });
            FunctionPlot.Model = _plotModel;
            UpdateGraph();
        }

        private double ParseDouble(string text)
        {
            string s = text.Replace(',', '.');
            return double.Parse(s, CultureInfo.InvariantCulture);
        }

        private bool TryParseDouble(string text, out double result)
        {
            string s = text.Replace(',', '.');
            return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        private void UpdateGraph()
        {
            if (_plotModel == null) return;
            _plotModel.Series.Clear();
            ResultTextBox.Clear();

            if (!TryParseDouble(X0TextBox.Text, out double x0) ||
                !TryParseDouble(XkTextBox.Text, out double xk) ||
                !TryParseDouble(DxTextBox.Text, out double dx) ||
                !TryParseDouble(DTextBox.Text, out double d))
            {
                ResultTextBox.Text = "Некорректные входные данные.";
                _plotModel.InvalidatePlot(true);
                return;
            }

            if (xk <= x0)
            {
                ResultTextBox.Text = "Xk должно быть больше X0.";
                _plotModel.InvalidatePlot(true);
                return;
            }
            if (dx <= 0)
            {
                ResultTextBox.Text = "Шаг Dx должен быть положительным.";
                _plotModel.InvalidatePlot(true);
                return;
            }

            double estimatedPoints = (xk - x0) / dx;
            if (estimatedPoints > 5000)
            {
                MessageBox.Show($"Слишком много точек для отображения ({estimatedPoints:F0}). Увеличьте шаг или уменьшите диапазон.", "Предупреждение");
                _plotModel.InvalidatePlot(true);
                return;
            }

            LineSeries series = new LineSeries();
            series.Title = "y = x² + tan(5x + d/x)";
            series.Color = OxyColors.Blue;

            bool hasErrors = false;
            int pointsAdded = 0;

            for (double x = x0; x <= xk; x += dx)
            {
                if (Math.Abs(x) < 1e-9)
                {
                    ResultTextBox.AppendText($"x = {x:F3}: деление на ноль (x = 0) невозможно\r\n");
                    hasErrors = true;
                    continue;
                }

                double argument = 5 * x + d / x;
                double tanValue = Math.Tan(argument);

                if (double.IsInfinity(tanValue) || double.IsNaN(tanValue))
                {
                    ResultTextBox.AppendText($"x = {x:F3}: тангенс не определён\r\n");
                    hasErrors = true;
                    continue;
                }

                double y = x * x + tanValue;

                if (Math.Abs(y) > 1e6)
                {
                    ResultTextBox.AppendText($"x = {x:F3}: значение y слишком велико\r\n");
                    hasErrors = true;
                    continue;
                }

                series.Points.Add(new OxyPlot.DataPoint(x, y));
                pointsAdded++;
                ResultTextBox.AppendText($"x = {x:F3}\t y = {y:F3}\r\n");
            }

            if (pointsAdded > 0)
            {
                _plotModel.Series.Add(series);
            }
            else
            {
                ResultTextBox.AppendText("Нет точек для построения графика.\r\n");
            }

            if (hasErrors)
            {
                ResultTextBox.AppendText("Некоторые точки были пропущены.\r\n");
            }

            _plotModel.InvalidatePlot(true);
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGraph();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateGraph();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            X0TextBox.Text = "0.1";
            XkTextBox.Text = "5";
            DxTextBox.Text = "0.2";
            DTextBox.Text = "1";
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