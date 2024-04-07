using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleRubiksCube
{

    struct Point
    {
        public int x;
        public int y;

        public Point(int x_, int y_)
        {
            x = x_;
            y = y_;
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Point a, Point b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.x + b.x, a.y + b.y);
        }
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.x - b.x, a.y - b.y);
        }
    }

    struct Point3
    {
        public int x;
        public int y;
        public int z;

        public Point3(int x_, int y_, int z_)
        {
            x = x_;
            y = y_;
            z = z_;
        }

        public static bool operator ==(Point3 a, Point3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Point3 a, Point3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }
        public static Point3 operator +(Point3 a, Point3 b)
        {
            return new Point3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Point3 operator -(Point3 a, Point3 b)
        {
            return new Point3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
    }

    class Vector2
    {
        public double x;
        public double y;

        public Vector2()
        {
            x = 0;
            y = 0;
        }
        public Vector2(double x_, double y_)
        {
            x = x_;
            y = y_;
        }
        public Vector2(Vector2 v)
        {
            x = v.x;
            y = v.y;
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static double[,] To2DArr(Vector2 p)
        {
            double[,] r = { { p.x, p.y } };
            return r;
        }
        public static Vector2 From2DArr(double[,] a)
        {
            Vector2 r = new Vector2(a[0, 0], a[0, 1]);
            return r;
        }

        public static double FindEuclidDistance(Vector2 a, Vector2 b)
        {
            return (double)Math.Sqrt(
                ((a.x - b.x) * (a.x - b.x)) +
                ((a.y - b.y) * (a.y - b.y))
                );
        }
    }

    class Vector3
    {
        public double x;
        public double y;
        public double z;

        public Vector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public Vector3(double x_, double y_, double z_)
        {
            x = x_;
            y = y_;
            z = z_;
        }
        public Vector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }


        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public override string ToString()
        {
            return x + ";" + y + ";" + z;
        }

        public static bool RangeEquals(Vector3 a, Vector3 b, double threshold)
        {
            Vector3 difference = new Vector3(
                Math.Abs(a.x - b.x),
                Math.Abs(a.y - b.y),
                Math.Abs(a.z - b.z)
                );
            return difference.x < threshold && difference.y < threshold && difference.z < threshold;
        }

        public static Vector3 CrossProduct(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
                );
        }

        public double[] ToArr()
        {
            double[] r = { x, y, z };
            return r;
        }

        public static double[,] To2DArr(Vector3 p)
        {
            double[,] r = { { p.x, p.y, p.z } };
            return r;
        }
        public static Vector3 From2DArr(double[,] a)
        {
            Vector3 r = new Vector3(a[0, 0], a[0, 1], a[0, 2]);
            return r;
        }

        public static Vector3 Rotate(Vector3 point, Vector3 origin, int dimension, double radians)
        {
            point -= origin;
            double sinA = (double)Math.Sin(radians);
            double cosA = (double)Math.Cos(radians);
            Vector3 result;
            if (dimension == 0)
            {
                result = new Vector3(point.x, point.y * cosA - point.z * sinA, point.y * sinA + point.z * cosA);
            }
            else if (dimension == 1)
            {
                result = new Vector3(point.x * cosA + point.z * sinA, point.y, -point.x * sinA + point.z * cosA);
            }
            else if (dimension == 2)
            {
                result = new Vector3(point.x * cosA - point.y * sinA, point.x * sinA + point.y * cosA, point.z);
            }
            else
            {
                return null;
            }
            return result + origin;
        }

        public static double FindEuclidDistance(Vector3 a, Vector3 b)
        {
            return (double)Math.Sqrt(
                ((a.x - b.x) * (a.x - b.x)) +
                ((a.y - b.y) * (a.y - b.y)) +
                ((a.z - b.z) * (a.z - b.z))
                );
        }


    }
}
