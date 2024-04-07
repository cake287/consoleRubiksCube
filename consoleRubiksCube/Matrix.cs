using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleRubiksCube
{
    class Matrix
    {
        public static double[,] Multiply(double[,] a, double[,] b)
        {
            double[,] c = new double[b.GetLength(0), a.GetLength(1)];
            // loop through all parts of C
            for (int y = 0; y <= c.GetLength(1) - 1; y++) // for each row
            {
                for (int x = 0; x <= c.GetLength(0) - 1; x++) // for each column
                {
                    double sum = 0;
                    for (int i = 0; i <= a.GetLength(0) - 1; i++) // loop through the row of A and the column of B
                    {
                        sum += a[i, y] * b[x, i];
                    }
                    c[x, y] = sum;
                }
            }
            return c;
        }
    }
}
