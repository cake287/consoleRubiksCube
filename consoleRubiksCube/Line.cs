using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleRubiksCube
{
    class Line
    {
        public Vector2[] points;
        public double[] equationCoefficients; // ax+by+c=0 where a = [0], b = [1], c = [2]

        public Line(Vector2[] _points)
        {
            points = _points;
            equationCoefficients = LineEquationFromPoints(_points);
        }

        double[] LineEquationFromPoints(Vector2[] points) // https://bobobobo.wordpress.com/2008/01/07/solving-linear-equations-ax-by-c-0/
        {
            double _a = points[0].y - points[1].y;
            double _b = points[1].x - points[0].x;
            double _c = (points[0].x * points[1].y) - (points[1].x * points[0].y);
            return new double[] { _a, _b, _c };
        }

        static public double SolveWithValue(double value, bool isValueX, double[] equationCoefficients) // if isValueX = true, value is the x value, and the function solves for y
        {
            // x = (-by-c)/a
            // y = (-ax-c)/b

            // r = (-d*value - c)/e  where d=a and e=b if isValueX, d=b and e=a if !isValueX

            double d = isValueX ? equationCoefficients[0] : equationCoefficients[1];
            double e = isValueX ? equationCoefficients[1] : equationCoefficients[0];

            double r = (-d * value - equationCoefficients[2]) / e;
            return r;
        }
    }
}
