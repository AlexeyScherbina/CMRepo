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

namespace CMLab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataArray a;
        double[][] x = { new double[] { -4, 1, -5 },
                             new double[] { 0, 3, 2 },
                             new double[] { -1, 5, -2 }
            };
        public MainWindow()
        {
            InitializeComponent();
            InitTable(3, x);
        }
        private void InitTable(int n, double[][] A)
        {
            a = new DataArray(n, n);
            dataGridA.ItemsSource = a.Data.DefaultView;
            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    a[i][j] = x[i][j];
            dataGridA.CanUserAddRows = false;
        }

        public static bool Check(Matrix a, double[] ownVector, double ownValue)
        {
            double[] v = a * ownVector;
            for (int i = 0; i < v.Length; i++)
            {
                if (Math.Abs(v[i] - ownVector[i] * ownValue) >= 1e-6)
                    return false;
            }
            return true;
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < a.M; i++)
            {
                for (int j = 0; j < a.N; j++)
                {
                    x[i][j] = a[i][j];
                }
            }
            Matrix m = new Matrix(x);
            var answer = KrylovMethod.Solve(m);
            textBox.AppendText("Собственные числа \n");
            foreach (var item in answer.Item1)
                textBox.AppendText(Math.Round(item,5) + " ");
            textBox.AppendText("\n");
            textBox.AppendText("Матрица собственных векторов \n");
            for (int i = 0; i < answer.Item2.RowsCount; i++)
            {
                for (int j = 0; j < answer.Item2.ColumnsCount; j++)
                {
                    textBox.AppendText(Math.Round(answer.Item2[i][j], 5) + "  ");
                    for (int k = 0; k < 7 - Convert.ToString(Math.Round(answer.Item2[i][j], 5)).Length; k++)
                    {
                        textBox.AppendText("  ");
                    }
                }
                textBox.AppendText("\n");
            }  
        }
    }
}
