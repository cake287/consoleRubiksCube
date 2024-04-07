using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleRubiksCube
{
    class Face
    {
        public Vector3[] points;
        public int shadeCol;
        public bool drawEdges;
        public int edgeCol;
        public Face(Vector3[] points_, int shadeCol_, int edgeCol_, bool drawEdges_)
        {
            points = new Vector3[points_.Length];
            for (int i = 0; i <= points.Length - 1; i++)
            {
                points[i] = new Vector3(points_[i]);
            }
            points = points_;
            shadeCol = shadeCol_;
            edgeCol = edgeCol_;
            drawEdges = drawEdges_;
        }
        public Face(Face face)
        {
            points = new Vector3[face.points.Length];
            for (int i = 0; i <= face.points.Length - 1; i++)
            {
                points[i] = new Vector3(face.points[i]);
            }
            shadeCol = face.shadeCol;
            edgeCol = face.edgeCol;
            drawEdges = face.drawEdges;
        }

        //public void Rotate(Vector3 origin, int dimension, double radians)
        //{
        //    for (int i = 0; i <= points.Length - 1; i++)
        //    {
        //        points[i] = Vector3.Rotate(points[i], origin, dimension, radians);
        //    }
        //}
        //public void Translate(Vector3 translation)
        //{
        //    for (int i = 0; i <= points.Length - 1; i++)
        //    {
        //        points[i] += translation;
        //    }
        //}


        public static double FindZIntersectionOfPlane(double[] equationConstants, Vector2 xyVals) // https://math.stackexchange.com/a/2686620
        {
            double zIntersection = -((equationConstants[0] * xyVals.x) + (equationConstants[1] * xyVals.y) + equationConstants[3]) / equationConstants[2]; // solve equation for z
            return zIntersection;
        }
        public static double[] FindEquationOfPlane(Vector3[] pointsOnPlane) // Returns [a, b, c, d] where ax + by + cz + d = 0
        {
            double[] equationConstants = new double[4];

            Vector3 planeVectorA = pointsOnPlane[1] - pointsOnPlane[0];
            Vector3 planeVectorB = pointsOnPlane[2] - pointsOnPlane[0];
            Vector3 normalVector = Vector3.CrossProduct(planeVectorA, planeVectorB);
            equationConstants[0] = normalVector.x;
            equationConstants[1] = normalVector.y;
            equationConstants[2] = normalVector.z;
            equationConstants[3] = // substitute a point into the equation and solve for d
                -((equationConstants[0] * pointsOnPlane[0].x) +
                (equationConstants[1] * pointsOnPlane[0].y) +
                (equationConstants[2] * pointsOnPlane[0].z));

            return equationConstants;
        }
    }
}
