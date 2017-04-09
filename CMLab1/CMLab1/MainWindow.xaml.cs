using System;
using System.Collections.Generic;
using System.Data;
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

namespace CMLab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataArray a;
        DataArray b;
        public delegate string SolveMethod(double[,] A,double[] B, int n);
        SolveMethod sm;
        double[,] A1 = new double[4, 4] {
                { -0.93, -0.08, 0.11, -0.18 },
                { 0.18, -0.48, 0, 0.21 },
                { 0.13, 0.31, -1.0, -0.21 },
                { 0.08, 0, -0.33, -0.72 }};
        double[] b1 = new double[4] { 0.51, -1.17, 1.02, 0.28 };
        double[,] A2 = new double[3, 3] {
                { 5.4, -2.3, 3.4 },
                { 4.2, 1.7, -2.3,},
                { 3.4, 2.4, 7.4,}};
        double[] b2 = new double[3] { -3.5, 2.7, 1.9 };

        public MainWindow()
        {
            InitializeComponent();
            InitTable(4,A1,b1);
            sm = new SolveMethod(Methods.SimpleIterations);
        }

        private void InitTable(int n, double[,] A, double[] B)
        {
            a = new DataArray(n, n);
            dataGridA.ItemsSource = a.Data.DefaultView;
            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    a[i][j] = A[i,j];
            dataGridA.CanUserAddRows = false;
            b = new DataArray(n, 1);
            dataGridB.ItemsSource = b.Data.DefaultView;
            for (int i = 0; i < a.M; i++)
                    b[i][0] = B[i];
            dataGridB.CanUserAddRows = false;
        }

        private void SimpleIterationsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                HoletskyCheckBox.IsChecked = false;
                sm = new SolveMethod(Methods.SimpleIterations);
            }
            catch (Exception) { }
        }

        private void HoletskyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                SimpleIterationsCheckBox.IsChecked = false;
                sm = new SolveMethod(Methods.Holetsky);
            }
            catch (Exception) { }      
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string s = "";
            double[,] A = new double[a.M,a.N] ;
            double[] B = new double[b.M];
            for (int i = 0; i < a.M; i++)
            {
                for (int j = 0; j < a.N; j++)
                {
                    A[i, j] = a[i][j];
                }
                B[i] = b[i][0];
            }
            s = sm(A, B, B.Length);
            textBox.Text = s;
        }
        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                InitTable(4, A1, b1);
                checkBox2.IsChecked = false;
            }
            catch (Exception) { }
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                InitTable(3, A2, b2);
                checkBox1.IsChecked = false;
            }
            catch (Exception) { }
        }
    }
}
