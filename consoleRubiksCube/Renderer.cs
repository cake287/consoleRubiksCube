using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace consoleRubiksCube
{
    class Renderer
    {
        public double[,] projection = { { 1, 0, 0 },
                                { 0, 1, 0 } };
        public double renderDepth = double.PositiveInfinity;

        public void RenderFaces(ref ConsoleChar[,] image, Face[] faces)
        {
            double[,] imageZBuffer = new double[image.GetLength(0), image.GetLength(1)];
            for (int x = 0; x <= imageZBuffer.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= imageZBuffer.GetLength(1) - 1; y++)
                {
                    imageZBuffer[x, y] = renderDepth;
                }
            }

            foreach (Face face in faces)
            {
                Vector2[] projectedPoints = new Vector2[face.points.Length];
                for (int i = 0; i <= projectedPoints.Length - 1; i++)
                {
                    //projectedPoints[i] = Vector2.From2DArr(Matrix.Multiply(projection, Vector3.To2DArr(face.points[i])));
                    projectedPoints[i] = new Vector2(face.points[i].x, face.points[i].y);
                }

                for (int i = 1; i <= projectedPoints.Length - 2; i++) // draws triangles with points (0, i, i + 1). may not work if shape is concave
                {
                    int[,] buffer = new int[image.GetLength(0), image.GetLength(1)]; // 0 = transparent; 1 = face; 2 = edge

                    Drawing.DrawTriangle(ref buffer, new Vector2[] { projectedPoints[0], projectedPoints[i], projectedPoints[i + 1] }, 1);

                    if (face.drawEdges)
                    {
                        Drawing.DrawLine(ref buffer, new Line(new Vector2[] { projectedPoints[i], projectedPoints[i + 1] }), 2);
                        if (i == 1) // if the triangle drawn has a vertex adjacent to the 0th one, needs to draw line between 0 and the adjacent vertex
                        {
                            Drawing.DrawLine(ref buffer, new Line(new Vector2[] { projectedPoints[0], projectedPoints[i] }), 2);
                        }
                        else if (i == projectedPoints.Length - 2)
                        {
                            Drawing.DrawLine(ref buffer, new Line(new Vector2[] { projectedPoints[0], projectedPoints[i + 1] }), 2);
                        }
                    }

                    double[] equationConstants = Face.FindEquationOfPlane(new Vector3[] { face.points[0], face.points[i], face.points[i + 1] });
                    for (int x = 0; x <= buffer.GetLength(0) - 1; x++)
                    {
                        for (int y = 0; y <= buffer.GetLength(1) - 1; y++)
                        {
                            if (buffer[x, y] != 0)
                            {
                                double z = Face.FindZIntersectionOfPlane(equationConstants, new Vector2(x, y));
                                if (z < imageZBuffer[x, y])
                                {
                                    imageZBuffer[x, y] = z;
                                    if (buffer[x, y] == 1)
                                    {
                                        image[x, y].backColour = face.shadeCol;
                                    }
                                    else if (buffer[x, y] == 2)
                                    {
                                        image[x, y].backColour = face.edgeCol;
                                    }
                                }
                            }
                        }
                    }
                }


            }
        }
    }
}
