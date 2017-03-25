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

namespace CMLab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate double Function(double a, double b, double e, out string s);

        double width = 0;
        double height = 0;
        double YMax = 1;
        double YMin = -1;
        double XMin = -1;
        double XMax = 1;
        double xScale = 0;
        double yScale = 0;
        double x0 = 0;
        double y0 = 0;
        Function F;
        List<double> roots;

        public MainWindow()
        {
            InitializeComponent();
            roots = new List<double>();
            F = Methods.Chorde;
            GraphicParams();
        }
        void GraphicParams()
        {
            width = canvas.ActualWidth;
            height = canvas.ActualHeight;
            double.TryParse(textBoxYmin.Text, out YMin);
            double.TryParse(textBoxYmax.Text, out YMax);
            double.TryParse(textBoxXmin.Text, out XMin);
            double.TryParse(textBoxXmax.Text, out XMax);
            xScale = width / (XMax - XMin);
            yScale = height / (YMax - YMin);
            x0 = -XMin * xScale;
            y0 = YMax * yScale;
        }

        void AddLine(Brush stroke, double x1, double y1, double x2, double y2)
        {
            canvas.Children.Add(new Line() { X1 = x1, X2 = x2, Y1 = y1, Y2 = y2, Stroke = stroke });
        }

        void AddText(string text, double x, double y)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = Brushes.Black;
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            canvas.Children.Add(textBlock);
        }

        void DrawGraph()
        {
            canvas.Children.Clear();
            int margin = 0;
            canvas.Children.Add(new Rectangle()
            {
                Width = width,
                Height = height,
                Margin = new Thickness(0, 0, 0, 0),
                Fill = Brushes.Wheat,
                Opacity = 0.01
            });
            double xStep = 1;
            while (xStep * xScale < 25)
                xStep *= 10;
            while (xStep * xScale > 250)
                xStep /= 10;
            for (double dx = xStep; dx <= XMax; dx += xStep)
            {
                double x = x0 + dx * xScale;
                AddLine(Brushes.LightGray, x + margin, 0, x + margin, height);
                AddText(string.Format("{0:0.###}", dx), x + 1 + margin, y0 - 2);
            }
            for (double dx = -xStep; dx >= XMin; dx -= xStep)
            {
                double x = x0 + dx * xScale;
                AddLine(Brushes.LightGray, x + margin, 0, x + margin, height);
                AddText(string.Format("{0:0.###}", dx), x + 1 + margin, y0 - 2);
            }
            double yStep = 1;
            while (yStep * yScale < 20)
                yStep *= 10;
            while (yStep * yScale > 200)
                yStep /= 10;
            for (double dy = yStep; dy <= YMax; dy += yStep)
            {
                double y = y0 - dy * yScale;
                AddLine(Brushes.LightGray, 0 + margin, y, width + margin, y);
                if (y < height - 12)
                {
                    AddText(string.Format("{0:0.###}", dy), x0 + 2 + margin, y - 2);
                }
            }
            for (double dy = -yStep; dy >= YMin; dy -= yStep)
            {
                double y = y0 - dy * yScale;
                AddLine(Brushes.LightGray, 0 + margin, y, width + margin, y);
                if (y < height - 12)
                {
                    AddText(string.Format("{0:0.###}", dy), x0 + 2 + margin, y - 2);
                }
            }
            if (XMin * XMax < 0)
            {
                AddLine(Brushes.Black, x0 + margin, 0, x0 + margin, height);
                AddText("Y", x0 - 10 + margin, 2);
            }
            else
            {
                AddLine(Brushes.Black, margin, 0, margin, height);
                AddText("Y", 4, 2);
                for (double dy = yStep; dy < YMax; dy += yStep)
                {
                    double y = y0 - dy * yScale;
                    if (y < height - 12)
                    {
                        AddText(string.Format("{0:0.###}", dy), margin + 2, y - 2);
                    }
                }
                for (double dy = -yStep; dy > YMin; dy -= yStep)
                {
                    double y = y0 - dy * yScale;
                    AddText(string.Format("{0:0.###}", dy), margin + 2, y - 2);
                }
            }
            if (YMin * YMax < 0)
            {
                AddLine(Brushes.Black, 0 + margin, y0, width + margin, y0);
                AddText("X", width - 10 + margin, y0 - 14);
            }
            else
            {
                AddLine(Brushes.Black, 0 + margin, height, width + margin, height);
                AddText("X", width - 10 + margin, height - 14);
                for (double dx = xStep; dx <= XMax; dx += xStep)
                {
                    double x = x0 + dx * xScale;
                    if (x > -1)
                    {
                        AddText(string.Format("{0:0.###}", dx), x + 1 + margin, height - 20);
                    }
                }
                for (double dx = -xStep; dx >= XMin; dx -= xStep)
                {
                    double x = x0 + dx * xScale;
                    if (x > -1)
                    {
                        AddText(string.Format("{0:0.###}", dx), x + 1 + margin, height - 20);
                    }
                }
            }
            AddText("0", x0 + 1 + margin, y0 - 2);
        }
        void DrawRoot(double x)
        {
            canvas.Children.Add(new Ellipse()
            {
                Width = 6,
                Height = 6,
                Margin = new Thickness(x0 + x * xScale - 3, y0 - 3, 0, 0),
                Fill = Brushes.Black
            });
        }
        void DrawGraphic()
        {
            int margin = 0;
            double e = double.Parse(textBoxE.Text);
            if (e < 0.01) e = 0.01;
            double x = XMin;
            double y = Methods.Func(x);
            for (double i = XMin + e; i <= XMax; i += e)
            {
                try
                {
                    canvas.Children.Add(new Line()
                    {
                        X1 = x0 + margin + x * xScale,
                        X2 = x0 + margin + i * xScale,
                        Y1 = y0 - y * yScale,
                        Y2 = y0 - Methods.Func(i) * yScale,
                        StrokeThickness = 2,
                        Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0))
                    });
                    x = i;
                    y = Methods.Func(i);
                }
                catch (Exception) { }
            }
        }

        private void checkBoxHorde_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                checkBoxSimpleIter.IsChecked = false;
                F = Methods.Chorde;
            }
            catch (Exception) { }
        }

        private void checkBoxSimpleIter_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                checkBoxHorde.IsChecked = false;
                F = Methods.SimpleIter;
            }
            catch (Exception) { }
        }

        private void buttonSolve_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double eps = double.Parse(textBoxE.Text);
                textBoxResult.Text = "Корни: \n";
                int count = 0;
                GraphicParams();
                DrawGraph();
                DrawGraphic();
                string s = "";
                double step = 0.7;
                for (double i = XMin; i < XMax; i += step)
                {
                    if (Methods.Func(i) * Methods.Func(i + step) < 0)
                    {
                        double res = F(i, i + step, eps, out s);
                        roots.Add(res);
                        count++;
                        textBoxResult.AppendText(s);
                        textBoxResult.AppendText("x" + count + " = " + Math.Round(res, textBoxE.Text.Length) + "\n\n");
                        DrawRoot(res);
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
