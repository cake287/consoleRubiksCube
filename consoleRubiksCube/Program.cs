using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace consoleRubiksCube
{
    class Program
    {
        static Vector3 imageOffset;
        static Renderer renderer;
        static ConsoleChar[,] image;
        static ConsoleChar[,] backgroundImage;
        static Vector3 cubeRot;
        static int backColour = 7;
        static int backgroundForecol = 7;
        static int backgroundBackcol = 8;
        static void Main(string[] args)
        {
            Console.TreatControlCAsInput = true; // treats control + a as input

            Maximize();
            renderer = new Renderer();            

            image = new ConsoleChar[50, 50];
            for (int x = 0; x <= image.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= image.GetLength(1) - 1; y++)
                {
                    image[x, y] = new ConsoleChar(' ', 7, backColour);
                }
            }

            backgroundImage = new ConsoleChar[image.GetLength(0) * 2 + 60, image.GetLength(1)];
            for (int x = image.GetLength(0) * 2; x <= backgroundImage.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= backgroundImage.GetLength(1) - 1; y++)
                {
                    backgroundImage[x, y] = new ConsoleChar(' ', backgroundForecol, backgroundBackcol);
                }
            }

            string[] sideTextLines = new string[] {
                "Console Rubik's Cube - Eddie Smith 2019",
                "",
                "Navigation",
                "Numpad4 - Rotate Left",
                "Numpad6 - Rotate Right",
                "Numpad8 - Rotate Up",
                "Numpad2 - Rotate Down",
                "Numpad1 - Roll Left",
                "Numpad3 - Roll Right",
                "",
                "Face Rotation",
                "Shift + face key - -90 degree rotation",
                "Control + face key - 180 degree rotation",
                "W - Top Face", //White
                "S - Bottom Face", //Yellow
                "Q - Front Face", //Green
                "E - Back Face", //Blue
                "A - Left Face", //Orange
                "D - Right Face", //Red
                "",
                "Other",
                "U - Change animation speed",
                "G - Algorithm menu",
                //"T - Scramble" // not currently working
            };
            for (int y = 0; y <= sideTextLines.Length - 1; y++)
            {
                ConsoleChar[] lineChars = ConsoleChar.StrToConsoleCharArr(sideTextLines[y], backgroundForecol, backgroundBackcol);
                for (int x = 0; x <= lineChars.Length - 1; x++)
                {
                    backgroundImage[image.GetLength(0) * 2 + x + 1, y + 1] = lineChars[x];
                }
            }



            int cubeDimension = 3;
            int cubieSideLength = 10;
            int cubeSize = cubeDimension * cubieSideLength;
            int offset = cubeSize / 2;

            CubeFace[] cubeFaces = new CubeFace[6];
            cubeFaces[0] = new CubeFace(new Vector3(0, -offset, 0), 1, -Math.PI / 2);
            cubeFaces[1] = new CubeFace(new Vector3(0, offset, 0), 1, Math.PI / 2);
            cubeFaces[2] = new CubeFace(new Vector3(0, 0, -offset), 2, -Math.PI / 2);
            cubeFaces[3] = new CubeFace(new Vector3(0, 0, offset), 2, Math.PI / 2);
            cubeFaces[4] = new CubeFace(new Vector3(-offset, 0, 0), 0, -Math.PI / 2);
            cubeFaces[5] = new CubeFace(new Vector3(offset, 0, 0), 0, Math.PI / 2);

            Cubie[,,] cubies = new Cubie[cubeDimension, cubeDimension, cubeDimension];
            for (int x = 0; x <= cubeDimension - 1; x++)
            {
                for (int y = 0; y <= cubeDimension - 1; y++)
                {
                    for (int z = 0; z <= cubeDimension - 1; z++)
                    {
                        Vector3 position = new Vector3(x * cubieSideLength - offset, y * cubieSideLength - offset, z * cubieSideLength - offset);
                        cubies[x, y, z] = new Cubie(position, cubieSideLength);
                        if (x == 0)
                        {
                            cubies[x, y, z].colours[4] = 6;
                        }
                        else if (x == cubeDimension - 1)
                        {
                            cubies[x, y, z].colours[5] = 4;
                        }
                        if (y == 0)
                        {
                            cubies[x, y, z].colours[0] = 14;
                        }
                        else if (y == cubeDimension - 1)
                        {
                            cubies[x, y, z].colours[1] = 15;
                        }
                        if (z == 0)
                        {
                            cubies[x, y, z].colours[2] = 10;
                        }
                        else if (z == cubeDimension - 1)
                        {
                            cubies[x, y, z].colours[3] = 9;
                        }
                    }
                }
            }
            
            imageOffset = new Vector3(image.GetLength(0) / 2, image.GetLength(1) / 2, 0);

            double rotateStep = Math.PI / 180 * 5;
            cubeRot = new Vector3();

            int lerpFrames = 10;
            int faceRotations = 1;
            Vector3 faceToRotate = new Vector3();
            bool shouldLoop = true;
            bool shouldDraw = true;
            bool shouldInput = true; // so that if text input is needed (i.e. animation frames input), the user does not have to press enter then input again for the cube to be drawn
            Vector3 dir = new Vector3(5, -5, 0);
            do
            {
                if (shouldDraw)
                {
                    cubeRot += new Vector3(dir.y * rotateStep, dir.x * rotateStep, dir.z * rotateStep);

                    RenderCube(ref cubies);
                }

                ConsoleKeyInfo input;
                if (shouldInput)
                {
                    input = Console.ReadKey(true);                    
                } else
                {
                    input = new ConsoleKeyInfo('a', ConsoleKey.F23, false, false, false);
                    shouldInput = true;
                }
                ConsoleKey inputKey = input.Key;

                shouldDraw = true;
                dir = new Vector3();
                faceToRotate = new Vector3();

                faceRotations = 1;
                if (input.Modifiers.HasFlag(ConsoleModifiers.Shift)) { faceRotations = -1; }
                else if (input.Modifiers.HasFlag(ConsoleModifiers.Control)) { faceRotations = 2; }

                if (inputKey == ConsoleKey.NumPad2) { dir.y = 1; }
                else if (inputKey == ConsoleKey.NumPad8) { dir.y = -1; }
                else if (inputKey == ConsoleKey.NumPad6) { dir.x = 1; }
                else if (inputKey == ConsoleKey.NumPad4) { dir.x = -1; }
                else if (inputKey == ConsoleKey.NumPad1) { dir.z = 1; }
                else if (inputKey == ConsoleKey.NumPad3) { dir.z = -1; }

                else if (inputKey == ConsoleKey.S) { faceToRotate.y = -1; }
                else if (inputKey == ConsoleKey.W) { faceToRotate.y = 1; }
                else if (inputKey == ConsoleKey.Q) { faceToRotate.z = -1; }
                else if (inputKey == ConsoleKey.E) { faceToRotate.z = 1; }
                else if (inputKey == ConsoleKey.A) { faceToRotate.x = -1; }
                else if (inputKey == ConsoleKey.D) { faceToRotate.x = 1; }

                else if (inputKey == ConsoleKey.G) { AlgMenu(); }
                //else if (inputKey == ConsoleKey.T) { Scramble(ref cubies, ref cubeFaces); }
                
                else if (inputKey == ConsoleKey.U)
                {
                    Console.Clear();
                    lerpFrames = InputInt("Enter animation speed (frames per turn): ", 1, 500);
                    shouldInput = false;
                }

                else if (inputKey == ConsoleKey.Escape)
                {
                    shouldLoop = false;
                }
                else
                {
                    shouldDraw = false;
                }

                if (faceToRotate != new Vector3())
                {
                    cubeFaces[TransformFaceIndex(faceToRotate, cubeRot)].RotateFace(ref cubies, faceRotations, lerpFrames);
                }
            } while (shouldLoop);
        }

        public static void RenderCube(ref Cubie[,,] cube)
        {
            Face[] drawingFaces = new Face[cube.GetLength(0) * cube.GetLength(1) * cube.GetLength(2) * 6];
            int faceIndex = 0;
            for (int x = 0; x <= cube.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= cube.GetLength(1) - 1; y++)
                {
                    for (int z = 0; z <= cube.GetLength(2) - 1; z++)
                    {
                        Face[] faces = cube[x, y, z].GetFaces(cubeRot.y, cubeRot.x, cubeRot.z, imageOffset);
                        for (int i = 0; i <= 5; i++)
                        {
                            drawingFaces[faceIndex] = faces[i];
                            faceIndex++;
                        }
                    }
                }
            }

            renderer.RenderFaces(ref image, drawingFaces);
            ConsoleChar[,] adjustedImage = ConsoleChar.Flip2DArrUpDown(ConsoleChar.doubleWidth2DArr(image));

            ConsoleChar[,] outputImage = new ConsoleChar[backgroundImage.GetLength(0), backgroundImage.GetLength(1)];
            for (int x = 0; x <= backgroundImage.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= backgroundImage.GetLength(1) - 1; y++)
                {
                    outputImage[x, y] = backgroundImage[x, y];
                }
            }
            for (int x = 0; x <= adjustedImage.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= adjustedImage.GetLength(1) - 1; y++)
                {
                    outputImage[x, y] = adjustedImage[x, y];
                }
            }
            ConsoleBuffer.draw(outputImage);

            for (int x = 0; x <= image.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= image.GetLength(1) - 1; y++)
                {
                    image[x, y].backColour = backColour;
                }
            }
        }

        static void Scramble(ref Cubie[,,] cube, ref CubeFace[] cubeFaces)
        {
            Random rnd = new Random();
            int scrambleDepth = 25;

            for (int i = 0; i <= scrambleDepth - 1; i++)
            {
                int face = rnd.Next(cubeFaces.Length - 1); 
                int rotations = rnd.Next(1, 3); // 90, 180 or 270 degrees
                if (rotations == 3) { rotations = -1; } // required for an unknown reason
                Debug.WriteLine(face + " " + rotations);
                cubeFaces[face].RotateFace(ref cube, rotations);
                
            }
        }

        static int TransformFaceIndex(Vector3 inputFace, Vector3 rot)
        {
            Vector3 snappedRot = new Vector3(RoundToNearestRightAngle(rot.x), RoundToNearestRightAngle(rot.y), RoundToNearestRightAngle(rot.z));
            Vector3 reversedRot = new Vector3() - snappedRot;

            Vector3 newInputFace = inputFace;
            newInputFace = Vector3.Rotate(newInputFace, new Vector3(), 0, reversedRot.x);
            newInputFace = Vector3.Rotate(newInputFace, new Vector3(), 1, reversedRot.y);
            newInputFace = Vector3.Rotate(newInputFace, new Vector3(), 2, reversedRot.z);

            Debug.WriteLine(rot + "  " + snappedRot + "  " + reversedRot + "  " + inputFace + "  " + newInputFace);

            int r = -1;
            if (Vector3.RangeEquals(newInputFace, new Vector3(0, -1, 0), 0.0001)) { r = 0; }
            else if (Vector3.RangeEquals(newInputFace, new Vector3(0, 1, 0), 0.0001)) { r = 1; }
            else if (Vector3.RangeEquals(newInputFace, new Vector3(0, 0, -1), 0.0001)) { r = 2; }
            else if (Vector3.RangeEquals(newInputFace, new Vector3(0, 0, 1), 0.0001)) { r = 3; }
            else if (Vector3.RangeEquals(newInputFace, new Vector3(-1, 0, 0), 0.0001)) { r = 4; }
            else if (Vector3.RangeEquals(newInputFace, new Vector3(1, 0, 0), 0.0001)) { r = 5; }
            else { Debug.WriteLine("newInputFace not recognised. newInputFace = " + newInputFace.ToString());  }

            return r;
        }

        static double RoundToNearestRightAngle(double rot)
        {
            rot *= 180 / Math.PI; // to degrees
            rot /= 90;
            rot = Math.Round(rot);
            rot *= 90;
            rot *= Math.PI / 180; // to radians
            return rot;
        }

        static int InputInt(string message, int min, int max)
        {
            Console.WriteLine();
            bool isInt = false;
            int i = 0;
            do
            {
                Console.Write(message);
                string userInput = Console.ReadLine();
                isInt = int.TryParse(userInput, out i);
                if (!isInt || i < min || i >= max)
                {
                    Debug.WriteLine(userInput);
                    Console.WriteLine("Enter an integer, equal to or above " + min + ", and below " + max + ".");
                }
            } while (!isInt || i < min || i >= max);
            return i;
        }

        static string AlgMenu()
        {
            Console.Clear();
            Algorithms alg = new Algorithms();
            for (int i = 0; i <= alg.sets.Length - 1; i++)
            {
                Console.WriteLine(i + " - " + alg.sets[i].name);
            }
            int chosenSet = InputInt("Enter the chosen algorithm set's corresponding number: ", 0, alg.sets.Length);
            Console.Clear();
            Console.WriteLine("Algortihm set: " + alg.sets[chosenSet].name);
            for (int i = 0; i <= alg.sets[chosenSet].algorithms.Length - 1; i++)
            {
                Console.WriteLine(alg.sets[chosenSet].algorithms[i].name + " - " + alg.sets[chosenSet].algorithms[i].alg);
            }
            return "none";
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);
        private static void Maximize() //https://stackoverflow.com/a/22053200
        {
            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
        }
    }
}
