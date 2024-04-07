using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleRubiksCube
{
    class ConsoleChar
    {
        public char character;
        public int foreColour; // 0 - 15: https://docs.microsoft.com/en-us/dotnet/api/system.consolecolor?view=netframework-4.8#fields
        public int backColour;

        public ConsoleChar()
        {
            character = ' ';
            foreColour = 15;
            backColour = 0;
        }
        public ConsoleChar(char character_)
        {
            character = character_;
            foreColour = 15;
            backColour = 0;
        }
        public ConsoleChar(char character_, int foreColour_, int backColour_)
        {
            character = character_;
            foreColour = foreColour_;
            backColour = backColour_;
        }


        public static ConsoleBuffer.CharInfo ConsoleCharToCharInfo(ConsoleChar consoleChar)
        {
            ConsoleBuffer.CharInfo r = new ConsoleBuffer.CharInfo();
            r.Attributes = (short)(consoleChar.backColour * 16 + consoleChar.foreColour);
            r.Char.UnicodeChar = consoleChar.character;

            byte asciiChar = Encoding.Unicode.GetBytes(new char[] { consoleChar.character })[0];
            if (consoleChar.character == '┌') { asciiChar = 218; } //http://www.softwareandfinance.com/CSharp/PrintASCII.html stupid encoding - probably a conversion function but i couldn't find it
            else if (consoleChar.character == '┐') { asciiChar = 191; }
            else if (consoleChar.character == '└') { asciiChar = 192; }
            else if (consoleChar.character == '┘') { asciiChar = 217; }
            else if (consoleChar.character == '─') { asciiChar = 196; }
            else if (consoleChar.character == '│') { asciiChar = 179; }


            else if (consoleChar.character == '░') { asciiChar = 176; }
            else if (consoleChar.character == '▒') { asciiChar = 177; }
            else if (consoleChar.character == '▓') { asciiChar = 178; }
            r.Char.AsciiChar = asciiChar;

            return r;
        }
        public static ConsoleBuffer.CharInfo[] ConsoleCharToCharInfo(ConsoleChar[] consoleChars)
        {
            ConsoleBuffer.CharInfo[] r = new ConsoleBuffer.CharInfo[consoleChars.Length];
            for (int i = 0; i <= consoleChars.Length - 1; i++)
            {
                r[i] = ConsoleCharToCharInfo(consoleChars[i]);
            }
            return r;
        }

        public static ConsoleChar[] StrToConsoleCharArr(string str)
        {
            return StrToConsoleCharArr(str, 15, 0);
        }
        public static ConsoleChar[,] StrToConsoleCharArr(string[] stringArray, char spaceChar)
        {
            return StrToConsoleCharArr(stringArray, spaceChar, 15, 0);
        }
        public static ConsoleChar[] StrToConsoleCharArr(string str, int foreColour, int backColour)
        {
            ConsoleChar[] r = new ConsoleChar[str.Length];
            for (int i = 0; i <= str.Length - 1; i++)
            {
                r[i] = new ConsoleChar(str[i], foreColour, backColour);
            }
            return r;
        }
        public static ConsoleChar[,] StrToConsoleCharArr(string[] stringArray, char spaceChar, int foreColour, int backColour)
        {
            int maxLength = stringArray.Max(s => s.Length);
            ConsoleChar[,] r = new ConsoleChar[maxLength, stringArray.Length];

            for (int y = 0; y <= stringArray.Length - 1; y++)
            {
                for (int x = 0; x <= maxLength - 1; x++)
                {
                    r[x, y] = new ConsoleChar();
                    r[x, y].foreColour = foreColour;
                    r[x, y].backColour = backColour;
                    if (x <= stringArray[y].Length - 1)
                    {
                        r[x, y].character = stringArray[y][x];
                    }
                    else
                    {
                        r[x, y].character = spaceChar;
                    }
                }
            }

            return r;
        }

        public static ConsoleChar[] Arr2DTo1D(ConsoleChar[,] Arr2D)
        {
            ConsoleChar[] Arr1D = new ConsoleChar[Arr2D.GetLength(0) * Arr2D.GetLength(1)];
            int i = 0;
            for (int y = 0; y <= Arr2D.GetLength(1) - 1; y++)
            {
                for (int x = 0; x <= Arr2D.GetLength(0) - 1; x++)
                {
                    Arr1D[i++] = Arr2D[x, y];
                }
            }
            return Arr1D;
        }

        public static ConsoleChar[,] Flip2DArrUpDown(ConsoleChar[,] arr)
        {
            ConsoleChar[,] r = new ConsoleChar[arr.GetLength(0), arr.GetLength(1)];
            for (int x = 0; x <= arr.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= arr.GetLength(1) - 1; y++)
                {
                    r[x, y] = arr[x, arr.GetLength(1) - 1 - y];
                }
            }
            return r;
        }

        public static ConsoleChar[,] doubleWidth2DArr(ConsoleChar[,] arr)
        {
            ConsoleChar[,] r = new ConsoleChar[arr.GetLength(0) * 2, arr.GetLength(1)];
            for (int x = 0; x <= arr.GetLength(0) - 1; x++)
            {
                for (int y = 0; y <= arr.GetLength(1) - 1; y++)
                {
                    r[x * 2, y] = arr[x, y];
                    r[x * 2 + 1, y] = arr[x, y];
                }
            }
            return r;
        }
    }
}
