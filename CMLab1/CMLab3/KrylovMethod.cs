using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLab3
{
    using CharactericEqual = Utils.CharactericEqual;

    public static class KrylovMethod
    {
        public static Tuple<List<double>, Matrix, string> Solve(Matrix a)
        {
            double[][] y = new double[a.RowsCount][];
            double[] y0 = new double[a.RowsCount];
            y0[0] = 1;
            double[] yv = (double[])y0.Clone();
            for (int i = 0; i < a.RowsCount; i++){
                y[i] = a * yv;
                yv = y[i];
            }
            for (int i = 0; i < y.Length; i++)
            {
                for (int j = 0; j < y[i].Length; j++)
                {
                    y[i][j] *= -1;
                }
            }
            Matrix A = new Matrix(a.RowsCount, 0);
            for (int i = a.RowsCount - 2; i >= 0; i--){
                A.AddColumn(y[i]);
            }
            y0[0] = -1;
            A.AddColumn(y0);
            double[] b = new double[a.RowsCount];
            b = y[a.RowsCount - 1];
            double[] p = GaussMainElement.Solve(A.GetElements(), b);
            List<double> roots = new CharactericEqual(p.ToList()).GetRoots();

            Matrix q = Utils.GetOwnVectors(a, roots.ToArray());
            return new Tuple<List<double>, Matrix, string>(roots, q, null);
        }
    }
}
