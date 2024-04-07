using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace consoleRubiksCube
{
    class CubeFace
    {
        public Vector3 rotCentre;
        public int rotDimension;
        public double rotAngle;

        public CubeFace(Vector3 rotCentre_, int rotDimension_, double rotAngle_)
        {
            rotCentre = rotCentre_;
            rotDimension = rotDimension_;
            rotAngle = rotAngle_;
        }

        public void RotateFace(ref Cubie[,,] cube, int rotations, int lerpFrames)
        {
            List<Point3> cubieRefs = new List<Point3>();
            for (int x = 0; x <= cube.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= cube.GetLength(1) - 1; y++)
                {
                    for (int z = 0; z <= cube.GetLength(2) - 1; z++)
                    {
                        bool isOnFace = false;
                        for (int p = 0; p <= cube[x, y, z].points.Length - 1 && !isOnFace; p++)
                        {
                            if (Math.Abs(cube[x, y, z].points[p].ToArr()[rotDimension] - rotCentre.ToArr()[rotDimension]) < 0.001)
                            {
                                isOnFace = true;
                            }
                        }
                        if (isOnFace)
                        {
                            cubieRefs.Add(new Point3(x, y, z));
                        }
                    }
                }
            }

            double turnAngle = rotAngle * rotations;
            double lerpAngle = turnAngle / lerpFrames;
            for (int f = 0; f <= lerpFrames - 1; f++)
            {
                foreach (Point3 cubieRef in cubieRefs)
                {
                    for (int i = 0; i <= cube[cubieRef.x, cubieRef.y, cubieRef.z].points.Length - 1; i++)
                    {
                        cube[cubieRef.x, cubieRef.y, cubieRef.z].points[i] = Vector3.Rotate(cube[cubieRef.x, cubieRef.y, cubieRef.z].points[i], rotCentre, rotDimension, lerpAngle);
                    }
                    cube[cubieRef.x, cubieRef.y, cubieRef.z].position = Vector3.Rotate(cube[cubieRef.x, cubieRef.y, cubieRef.z].position, rotCentre, rotDimension, lerpAngle);
                }
                Program.RenderCube(ref cube);
            }

        }
        public void RotateFace(ref Cubie[,,] cube, int rotations)
        {
            double turnAngle = rotAngle * rotations;

            List<Point3> cubieRefs = new List<Point3>();            
            for (int x = 0; x <= cube.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= cube.GetLength(1) - 1; y++)
                {
                    for (int z = 0; z <= cube.GetLength(2) - 1; z++)
                    {
                        bool isOnFace = false;
                        for (int p = 0; p <= cube[x, y, z].points.Length - 1 && !isOnFace; p++)
                        {
                            if (cube[x, y, z].points[p].ToArr()[rotDimension] == rotCentre.ToArr()[rotDimension])
                            {
                                isOnFace = true;
                            }
                        }
                        if (isOnFace)
                        {
                            cubieRefs.Add(new Point3(x, y, z));
                        }
                    }
                }
            }

            foreach (Point3 cubieRef in cubieRefs)
            {
                for (int i = 0; i <= cube[cubieRef.x, cubieRef.y, cubieRef.z].points.Length - 1; i++)
                {
                    cube[cubieRef.x, cubieRef.y, cubieRef.z].points[i] = Vector3.Rotate(cube[cubieRef.x, cubieRef.y, cubieRef.z].points[i], rotCentre, rotDimension, turnAngle);
                }
                cube[cubieRef.x, cubieRef.y, cubieRef.z].position = Vector3.Rotate(cube[cubieRef.x, cubieRef.y, cubieRef.z].position, rotCentre, rotDimension, turnAngle);
            }
        }
    }
}
