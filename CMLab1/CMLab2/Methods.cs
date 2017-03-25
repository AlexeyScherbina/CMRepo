using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMLab1
{
    public static class Methods
    {
        public static double Func(double x)
        {
            return x*x*x + 3 * x*x - 3;
        }

        public static double Chorde(double x_prev, double x_curr, double e)
        {
            double x_next = 0;
            double tmp;

            do
            {
                tmp = x_next;
                x_next = x_curr - Func(x_curr) * (x_prev - x_curr) / (Func(x_prev) - Func(x_curr));
                x_prev = x_curr;
                x_curr = tmp;
            } while (Math.Abs(x_next - x_curr) > e);

            return x_next;
        }

        public static double SimpleIter(double a, double b, double e)
        {
            int k = 0;
            double x0, xk;
            x0 = (a + b) / 2;
            do
            {
                xk = Func(x0);
                if (Math.Abs(xk - x0) < e) break;
                else x0 = xk;

            }
            while (Math.Abs(a - x0) > e && Math.Abs(b - x0) > e);
            return xk;
        }
    }
}
