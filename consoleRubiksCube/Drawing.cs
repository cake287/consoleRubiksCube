using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleRubiksCube
{
    class Drawing
    {

        public static void DrawLine(ref ConsoleChar[,] image, Line l, int colour)
        {
            int[,] colourImage = new int[image.GetLength(0), image.GetLength(1)];
            int adjustedColour = colour + 1; // so that the background value is 0 and can be used for transparency while still having black as a drawable colour
            DrawLine(ref colourImage, l, adjustedColour);
            for (int x = 0; x <= colourImage.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= colourImage.GetLength(1) - 1; y++)
                {
                    if (colourImage[x, y] != 0)
                    {
                        image[x, y].backColour = colourImage[x, y] - 1;
                    }
                }
            }
        }
        public static void DrawLine(ref int[,] image, Line l, int colour)
        {
            if (Math.Abs(l.equationCoefficients[1]) > Math.Abs(l.equationCoefficients[0])) // only draws one point per x value, so if it needs more than one point, the program draws per y value instead.
            {
                double startX = Math.Min(l.points[0].x, l.points[1].x);
                double endX = Math.Max(l.points[1].x, l.points[0].x);

                for (double x = startX; x <= endX; x++)
                {
                    double yVal = Line.SolveWithValue(x, true, l.equationCoefficients);
                    if (TestBounds(new Vector2(x, yVal), image.GetLength(0), image.GetLength(1)))
                    {
                        image[(int)Math.Floor(x), (int)Math.Floor(yVal)] = colour;
                    }
                }
            }
            else
            {
                double startY = Math.Min(l.points[0].y, l.points[1].y);
                double endY = Math.Max(l.points[1].y, l.points[0].y);

                for (double y = startY; y <= endY; y++)
                {
                    double xVal = Line.SolveWithValue(y, false, l.equationCoefficients);
                    if (TestBounds(new Vector2(xVal, y), image.GetLength(0), image.GetLength(1)))
                    {
                        image[(int)Math.Floor(xVal), (int)Math.Floor(y)] = colour;
                    }
                }
            }
        }
        
        public static void DrawTriangle(ref ConsoleChar[,] image, Vector2[] points, int fillCol)
        {
            int[,] colourImage = new int[image.GetLength(0), image.GetLength(1)];
            DrawTriangle(ref colourImage, points, fillCol - 1); // -1 so that the background value is 0 and can be used for transparency while still having black as a drawable colour
            for (int x = 0; x <= colourImage.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= colourImage.GetLength(1) - 1; y++)
                {
                    if (colourImage[x, y] != 0)
                    {
                        image[x, y].backColour = colourImage[x, y] - 1;
                    }
                }
            }
        }
        public static void DrawTriangle(ref int[,] image, Vector2[] points, int fillCol)
        {
            bool isLinear = true;
            Line[] lines = new Line[points.Length];
            for (int i = 0; i <= lines.Length - 1; i++) // 0,1; 1,2; 2,0
            {
                int j = i + 1;
                if (j > lines.Length - 1)
                {
                    j = 0;
                }
                lines[i] = new Line(new Vector2[] { points[i], points[j] });

                int otherPointIndex = j + 1;
                if (otherPointIndex > lines.Length - 1)
                {
                    otherPointIndex = 0;
                }


                double solvedValue;
                double pointValue;
                if (lines[i].equationCoefficients[1] != 0) // if the line is vertical
                {
                    solvedValue = Line.SolveWithValue(points[otherPointIndex].x, true, lines[i].equationCoefficients);
                    pointValue = points[otherPointIndex].y;
                }
                else
                {
                    solvedValue = Line.SolveWithValue(points[otherPointIndex].y, false, lines[i].equationCoefficients);
                    pointValue = points[otherPointIndex].x;
                }

                isLinear = Math.Abs(solvedValue - pointValue) <= solvedValue * 0.001; // if the other point isn't on this line, the shape is non linear
            }

            if (!isLinear)
            {
                Vector2 minPoint = new Vector2();
                Vector2 maxPoint = new Vector2();
                for (int point = 0; point <= points.Length - 1; point++)
                {
                    minPoint.x = Math.Min(minPoint.x, points[point].x);
                    minPoint.y = Math.Min(minPoint.y, points[point].y);
                    maxPoint.x = Math.Max(maxPoint.x, points[point].x);
                    maxPoint.y = Math.Max(maxPoint.y, points[point].y);
                }


                bool[] testGreaterThan = new bool[lines.Length]; // corresponds to each point - if true, then condition is (solveWithX > y), else condition is (solveWithX < y)
                for (int i = 0; i <= lines.Length - 1; i++)
                {
                    int otherPointIndex = i + 2; // = 2, 0, 1: refers to the point which isn't a point of the line
                    if (otherPointIndex > lines.Length - 1)
                    {
                        otherPointIndex -= lines.Length;
                    }

                    if (points[otherPointIndex].y > Line.SolveWithValue(points[otherPointIndex].x, true, lines[i].equationCoefficients)) // if the y of the other point > y of the line solved with the x of the other point 
                    { // if the point is above the line, testGreaterThan[i] = true
                        testGreaterThan[i] = true;
                    }
                    else
                    {
                        testGreaterThan[i] = false;
                    }
                }               


                for (double x = minPoint.x; x <= maxPoint.x; x++)
                {
                    for (double y = minPoint.y; y <= maxPoint.y; y++)
                    {
                        bool shouldNotShade = false;
                        for (int i = 0; i <= lines.Length - 1; i++)
                        {
                            if (testGreaterThan[i]) // if the rest of the triangle is above the line 
                            {
                                if (!(Line.SolveWithValue(x, true, lines[i].equationCoefficients) < y))
                                {
                                    shouldNotShade = true;
                                }
                            }
                            else
                            {
                                if (!(Line.SolveWithValue(x, true, lines[i].equationCoefficients) > y))
                                {
                                    shouldNotShade = true;
                                }
                            }
                        }

                        if (!shouldNotShade)
                        {
                            if (TestBounds(new Vector2(x, y), image.GetLength(0), image.GetLength(1)))
                            {
                                image[(int)Math.Floor(x), (int)Math.Floor(y)] = fillCol;
                            }
                        }
                    }
                }
            }
        }
        
        static bool TestBounds(Vector2 point, double width, double height)
        {
            if (point.x >= 0 && point.x <= width - 1 && point.y >= 0 && point.y <= height - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
