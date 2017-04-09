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
        DataArray b;
        double[,] A1 = new double[3, 3] {
                { -4, 1, -5 },
                { 0, 3, 2,},
                {-1, 5, -2,}};
        double[] b1 = new double[3] { 1, 0, 0 };
        public MainWindow()
        {
            InitializeComponent();
            InitTable(3, A1,b1);
        }
        public static double[] Holetsky(double[,] A, double[] b, int n)
        {
            double[,] B = new double[n, n];
            double[,] C = new double[n, n];
            double[] y = new double[n];
            double[] x = new double[n];
            B = A;
            for (int j = 0; j < n; j++)
            {
                C[0, j] = A[0, j] / B[0, 0];
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //B
                    if (j == 0)
                    {
                    }
                    else
                    {
                        for (int k = 0; k < j; k++)
                        {
                            B[i, j] -= A[i, k] * C[k, j];
                        }
                    }
                    //C
                    if (i == 0)
                    {
                    }
                    else
                    {
                        C[i, j] = 1 / B[i, i];
                        double sum = 0;
                        for (int k = j; k < i; k++)
                        {
                            sum += B[i, k] * C[k, j];
                        }
                        C[i, j] *= (B[i, j] - sum);
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j > i)
                    {
                        B[i, j] = 0;
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    y[i] = b[i] / B[0, 0];
                }
                else
                {
                    y[i] = b[i];
                    for (int k = 0; k < i; k++)
                    {
                        y[i] -= B[i, k] * y[k];
                    }
                    y[i] /= B[i, i];
                }
            }
            x[n - 1] = y[n - 1];
            for (int i = n - 1; i > -1; i--)
            {
                if (i == n - 1)
                {
                }
                else
                {
                    x[i] = y[i];
                    for (int k = i + 1; k <= n - 1; k++)
                    {
                        x[i] -= C[i, k] * x[k];
                    }
                }
            }
            return x;
        }
        void Krilov(double[,] A,double[] y)
        {
            double[,] mas = new double[3, 3]
            {
                {0,0,y[0] },
                {0,0,y[1] },
                {0,0,y[2] }
            };
            double[] last = new double[3];
            double[] y1 = new double[3];
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    y1[i] += A[i, j] * y[j];
                }
            }
            double[] y2 = new double[3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    y2[i] += A[i, j] * y1[j];
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    last[i] += A[i, j] * y2[j];
                }
            }
            for (int i = 0; i < 3; i++)
            {
                mas[i, 1] = y1[i];
            }
            for (int i = 0; i < 3; i++)
            {
                mas[i, 0] = y2[i];
            }
            textBox.AppendText("Матрица собственных веторов: \n");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    textBox.AppendText(mas[i,j]+ "  ");
                }
                textBox.AppendText("\n");
            }
            textBox.AppendText("yn: \n");
            for (int i = 0; i < 3; i++)
            {
                textBox.AppendText(last[i] + "  ");
            }
            textBox.AppendText("\n");
            textBox.AppendText("Собственные числа матрицы: \n");
            double[] res = new double[3];
            res = Holetsky(mas, last, 3);
            for (int i = 0; i < 3; i++)
            {
                textBox.AppendText(res[i] + "  ");
            }
            textBox.AppendText("\n\n");
        }
        private void InitTable(int n, double[,] A, double[] B)
        {
            a = new DataArray(n, n);
            dataGridA.ItemsSource = a.Data.DefaultView;
            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    a[i][j] = A[i, j];
            dataGridA.CanUserAddRows = false;
            b = new DataArray(n, 1);
            dataGridB.ItemsSource = b.Data.DefaultView;
            for (int i = 0; i < a.M; i++)
                b[i][0] = B[i];
            dataGridB.CanUserAddRows = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            double[,] A = new double[a.M, a.N];
            double[] B = new double[b.M];
            for (int i = 0; i < a.M; i++)
            {
                for (int j = 0; j < a.N; j++)
                {
                    A[i, j] = a[i][j];
                }
                B[i] = b[i][0];
            }
            Krilov(A, B);
        }
    }
}
