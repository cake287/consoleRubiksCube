using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleRubiksCube
{
    public struct AlgorithmSet
    {
        public string name;
        public Algorithm[] algorithms;

        public AlgorithmSet(string name_, Algorithm[] algs_)
        {
            name = name_;
            algorithms = algs_;
        }
    }
    public struct Algorithm
    {
        public string name;
        public string alg;

        public Algorithm(string name_, string alg_)
        {
            name = name_;
            alg = alg_;
        }
    }
    
    public class Algorithms
    {
        public AlgorithmSet[] sets = new AlgorithmSet[] {
            new AlgorithmSet("PLL", GetPLLs()),
            new AlgorithmSet("OLL", GetOLLs())
        };

        public static Algorithm[] GetPLLs()
        {
            List<Algorithm> r = new List<Algorithm>();
            r.Add(new Algorithm("Ua", "M2 U M U2 M' U M2"));
            r.Add(new Algorithm("Ub", "M2 U' M U2 M' U' M2"));
            r.Add(new Algorithm("H", "M2 U M2 U2 M2 U M2 "));
            r.Add(new Algorithm("Z", "M2 U M2 U M' U2 M2 U2 M'"));

            r.Add(new Algorithm("Aa", "x (R' U R') D2 (R U' R') D2 R2 x'"));
            r.Add(new Algorithm("Ab", "x R2' D2 (R U R') D2 (R U' R) x'"));
            r.Add(new Algorithm("E", "x' (R U' R' D) (R U R' D') (R U R' D) (R U' R' D') x"));

            r.Add(new Algorithm("F", "U' (R' U' F') (R U R' U' R' F R2 U' R' U' R U R') U R"));
            r.Add(new Algorithm("T", "(R U R' U') R' F R2 U' R' U' (R U R') F'"));
            r.Add(new Algorithm("Ja (L)", "R' U L' U2 (R U' R') U2 R r"));
            r.Add(new Algorithm("Jb", "(R U R') F' (R U R' U') R' F R2 U' R'"));
            r.Add(new Algorithm("Na", "z U' R D' R2 U R' D U' R D' R2 U R' D"));
            r.Add(new Algorithm("Nb", "R U R' U R U R' F' R U R' U' R' F R2 U' R' U2 R U' R'"));
            r.Add(new Algorithm("Ra", "(R U R') F' (R U2 R') U2 R' F R U (R U2 R')"));
            r.Add(new Algorithm("Rb", "(R' U2 R) U2 R' F (R U R' U') R' F' R2 U'"));
            r.Add(new Algorithm("V ", "R' U R' U' y R' F' R2 U' R' U R' F R F"));

            r.Add(new Algorithm("Ga", "R2 U R' U R' U' R U' R2 D U' R' U R D'"));
            r.Add(new Algorithm("Gb", "R' U' R U D' R2 U R' U R U' R U' R2 D"));
            r.Add(new Algorithm("Gc", "R2 U' (R U' R U) (R' U R2 D') (U R U' R') D"));
            r.Add(new Algorithm("Gd", "(R U R' U') D R2 U' (R U' R') U R' U R2 D'"));
            return r.ToArray();
        }

        public static Algorithm[] GetOLLs()
        {
            List<Algorithm> r = new List<Algorithm>();
            r.Add(new Algorithm("testOLL", "U2"));
            return r.ToArray();
        }
    }
}
