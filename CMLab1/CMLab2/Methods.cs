using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLab2
{
    public static class Methods
    {
        public static double Func(double x)
        {
            return x*x*x + 3 * x*x - 3;
        }
        public static double Fi(double x,double c)
        {
            return x + c*Func(x);
        }
        public static double F1(double x)
        {
            return 3*x*x+6*x;
        }
        public static double Chorde(double a, double b, double e, out string s)
        {
            double x_next = 0;
            double tmp;
            int iter = 0;
            s = "";
            do
            {
                iter++;
                tmp = x_next;
                x_next = b - Func(b) * (a - b) / (Func(a) - Func(b));
                a = b;
                b = tmp;
                s += "Итерация " + iter + ": x = " + x_next + "\n";
            } while (Math.Abs(x_next - b) > e);

            return x_next;
        }

        public static double SimpleIter(double a, double b, double e, out string s)
        {
            double q = F1(b);
            q = F1(a) > q ? F1(a) : F1(b);
            double g = q/(1 - q);
            double x0;
            x0 = (a + b) / 2;
            int iter = 0;
            s = "";
            double xk;
            do
            {
                iter++;
                xk = Fi(x0, -1.0 / q);
                s += "Итерация " + iter + ": x = " + xk + "\n";
                if (Math.Abs(xk - x0) <= e) break;
                else x0 = xk;
            }
            while (iter < 100);
            return xk;
        }
    }
}
