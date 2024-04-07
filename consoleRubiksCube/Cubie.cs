using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleRubiksCube
{
    class Cubie
    {
        public Vector3 position; // -x -y -z corner of cubie of cubie
        public Vector3[] points;
        public int[] colours;
        int sideLength = 10;
        public Cubie(Vector3 position_, int sideLength_)
        {
            position = position_;
            sideLength = sideLength_;
            colours = new int[6];
            points = new Vector3[]
            {
                new Vector3(0,          0,          0),
                new Vector3(sideLength, 0,          0),
                new Vector3(sideLength, 0,          sideLength),
                new Vector3(0,          0,          sideLength),
                new Vector3(0,          sideLength,          0),
                new Vector3(sideLength, sideLength,          0),
                new Vector3(sideLength, sideLength,          sideLength),
                new Vector3(0,          sideLength,          sideLength)
            };
            for (int i = 0; i <= points.Length - 1; i++)
            {
                points[i] += position;
            }
            for (int i = 0; i <= colours.Length - 1; i++)
            {
                colours[i] = 8;
            }
        }

        public Face[] GetFaces(double rotY, double rotX, double rotZ, Vector3 imageOffset)
        {
            Vector3[] newPoints = new Vector3[points.Length];
            for (int i = 0; i <= newPoints.Length - 1; i++)
            {
                newPoints[i] = points[i];
                newPoints[i] = Vector3.Rotate(newPoints[i], new Vector3(), 2, rotZ);
                newPoints[i] = Vector3.Rotate(newPoints[i], new Vector3(), 1, rotY);
                newPoints[i] = Vector3.Rotate(newPoints[i], new Vector3(), 0, rotX);
                newPoints[i] += imageOffset;
            }

            Face[] r = new Face[]
            {
                new Face(new Vector3[] { newPoints[0], newPoints[1], newPoints[2], newPoints[3] }, colours[0], 0, true),
                new Face(new Vector3[] { newPoints[4], newPoints[5], newPoints[6], newPoints[7] }, colours[1], 0, true),
                new Face(new Vector3[] { newPoints[0], newPoints[1], newPoints[5], newPoints[4] }, colours[2], 0, true),
                new Face(new Vector3[] { newPoints[3], newPoints[2], newPoints[6], newPoints[7] }, colours[3], 0, true),
                new Face(new Vector3[] { newPoints[0], newPoints[3], newPoints[7], newPoints[4] }, colours[4], 0, true),
                new Face(new Vector3[] { newPoints[1], newPoints[2], newPoints[6], newPoints[5] }, colours[5], 0, true)
            };
            return r;
        }

        public void Rotate(Vector3 origin, int dimension, double radians)
        {
            for (int i = 0; i <= points.Length - 1; i++)
            {
                points[i] = Vector3.Rotate(points[i], origin, dimension, radians);
            }
            position = Vector3.Rotate(position, origin, dimension, radians);
        }
        public void Translate(Vector3 translation)
        {
            for (int i = 0; i <= points.Length - 1; i++)
            {
                points[i] += translation;
            }
        }
    }
}

//= new Vector3[][] {
//                new Vector3[] {
//                    new Vector3(0,          0,          0),
//                    new Vector3(0, sideLength, 0),
//                    new Vector3(sideLength, sideLength, 0),
//                    new Vector3(sideLength, 0,          0)
//                },
//                new Vector3[] {
//                    new Vector3(0,          0, sideLength),
//                    new Vector3(0, sideLength, sideLength),
//                    new Vector3(sideLength, sideLength, sideLength),
//                    new Vector3(sideLength, 0, sideLength)
//                },
//                new Vector3[] {
//                    new Vector3(0,          0,          0),
//                    new Vector3(0,          0, sideLength),
//                    new Vector3(sideLength, 0, sideLength),
//                    new Vector3(sideLength, 0,          0)
//                },
//                new Vector3[] {
//                    new Vector3(0, sideLength, 0),
//                    new Vector3(0, sideLength, sideLength),
//                    new Vector3(sideLength, sideLength, sideLength),
//                    new Vector3(sideLength, sideLength, 0)
//                },
//                new Vector3[] {
//                    new Vector3(0,          0,           0),
//                    new Vector3(0,          0, sideLength),
//                    new Vector3(0, sideLength, sideLength),
//                    new Vector3(0, sideLength,  0)
//                },
//                new Vector3[] {
//                    new Vector3(sideLength, 0,          0),
//                    new Vector3(sideLength, 0, sideLength),
//                    new Vector3(sideLength, sideLength, sideLength),
//                    new Vector3(sideLength, sideLength, 0)
//                }
//            };


            //for (int i = 0; i <= points.Length - 1; i++)
            //{
            //    for (int j = 0; j <= points[i].Length - 1; j++)
            //    {
            //        points[i][j] += position;
            //    }
            //    faces[i] = new Face(points[i], 8, 0, true);
            //}