using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLab1
{
    public static class Methods
    {
        public static double Norma(double[,] A, int n)
        {
            double[] x = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    x[i] += Math.Abs(A[i, j]);
                }
            }
            Array.Sort(x);
            return x[n-1];
        }
        public static double Condition(double[] x, double[] xp)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += Math.Pow(x[i] - xp[i], 2);
            }
            return Math.Sqrt(sum);
        }
        public static string SimpleIterations(double[,] A, double[] b, int n)
        {
            //добавить начальное приближение
            //проверить на сходимость и изменить вывод в этом случае
            double e = 0.001;
            double[] x = new double[n];
            double[] xp = new double[n];
            double[,] matrixA = new double[n, n];
            double[] matrixb = new double[n];
            string message = "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        matrixA[i, j] = 0;
                    }
                    else
                    {
                        matrixA[i, j] = -A[i, j] / A[i, i];
                    }
                }
                matrixb[i] = b[i] / A[i, i];
            }
            int counter = 0;
            do
            {
                counter++;
                xp = x;
                x = new double[n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        x[i] += matrixA[i, j] * xp[j];
                    }
                    x[i] += matrixb[i];
                }
                message += ("Итерация " + counter + "\n");
                for (int i = 0; i < n; i++)
                {
                    message += ("x" + (i + 1) + " = " + Math.Round(x[i],5) + "\n");
                }
                message += "\n";
            }
            while (Condition(x, xp) >= e * (1 - Norma(matrixA,n)) / Norma(matrixA,n) && counter < 20);
            return message;
        }

        public static double[,] Transpose(double[,] matrix)
        {
            double t;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = i; j < 3; ++j)
                {
                    t = matrix[i, j];
                    matrix[i, j] = matrix[j, i];
                    matrix[j, i] = t;
                }
            }
            return matrix;
        }

        public static string Holetsky(double[,] A, double[] b, int n)
        {
            double[,] B = new double[n, n];
            double[,] C = new double[n, n];
            double[] y = new double[n];
            double[] x = new double[n];
            string message = "";
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
            message += "Матрица B \n";
            Console.WriteLine("Матрица B");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    message += (Math.Round(B[i, j],5) + "  ");
                    Console.Write(B[i, j] + "  ");
                }
                message += "\n";
                Console.WriteLine();
            }
            message += "\n";
            Console.WriteLine();
            message += "Матрица C \n";
            Console.WriteLine("Матрица C");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    message += (Math.Round(C[i, j],5) + "  ");
                    Console.Write(C[i, j] + "  ");
                }
                message += "\n";
                Console.WriteLine();
            }
            message += "\n";
            Console.WriteLine();
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
            message += "Вектор у \n";
            Console.WriteLine("Вектор у");
            for (int i = 0; i < n; i++)
            {
                message += Math.Round(y[i],5) + "\n";
                Console.WriteLine(y[i]);
            }
            message += "\n";
            Console.WriteLine();
            x[n-1] = y[n-1];
            for (int i = n-1; i > -1; i--)
            {
                if (i == n-1)
                {
                }
                else
                {
                    x[i] = y[i];
                    for (int k = i + 1; k <= n-1; k++)
                    {
                        x[i] -= C[i, k] * x[k];
                    }
                }
            }
            message += "Вектор x \n";
            Console.WriteLine("Вектор x");
            for (int i = 0; i < n; i++)
            {
                message += Math.Round(x[i],5) + "\n";
                Console.WriteLine(x[i]);
            }
            message += "\n";
            Console.WriteLine();
            return message;
        }

    }
}
