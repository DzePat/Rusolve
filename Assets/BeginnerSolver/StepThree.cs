using BeginnerSolve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPhaseSolver;

namespace Assets.BeginnerSolver
{
    internal class StepThree
    {
        private static readonly Dictionary<(int, int), string> alignEdgeMoves = new()
        {
                {(8,8), "D' R' D R D F D' F' D2 D' R' D R D F D' F'"},
                {(8,9), "D L D' L' D' F' D F D2"},
                {(8,10), "D B D' B' D' L' D L D'"},
                {(8,11), "D R D' R' D' B' D B"},

                {(9,9), ""},
                {(9,10), ""},
                {(9,11), ""},

                {(10,10), ""},
                {(10,11), ""},

                {(11,11), ""},
        };

        private static readonly Dictionary<(int, int), string> centerEdgeMoves = new()
        {
            { (7,11), "D' D' B' D B D R D' R'" },
            { (7,39), "D R D' R' D' B' D B" },
            { (7,31), "D2 B D' B' D' L' D L"},
            { (7,35), "D' L' D L D B D' B'"},
            { (7,23), "D' L D' L' D' F' D F"},
            { (7,27), "F' D F D L D' L'"},
            { (7,19), "D R' D R D F D' F'"},
            { (7,15), "F D' F' D' R' D R"},       

            { (6,23), "D2 L D' L' D' F' D F" },
            { (6,27), "D' F' D F D L D' L'" },
            { (6,31), "D B D' B' D' L' D L"},
            { (6,35), "D' D' L' D L D' B D' B'"},
            { (6,11), "D' B' D B D R D' R'" },
            { (6,39), "R D' R' D' B' D B" },
            { (6,19), "R' D R D F D' F'" },
            { (6,15), "D' F D' F' D' R' D R" },

            { (5,19), "D' R' D R D F D' F'" },
            { (5,15), "D2 F D' F' D' R' D R" },
            { (5,23), "D L D' L' D' F' D F" },
            { (5,27), "D' D' F' D F D L D' L'" },
            { (5,11), "B' D B D R D' R'" },
            { (5,39), "D' R D' R' D' B' D B" },
            { (5,31), "B D' B' D' L' D L"},
            { (5,35), "D L' D L D B D' B'"},

            { (4,19), "D' D' R' D R D F D'" },
            { (4,15), "D F D' F' D' R' D R" },
            { (4,11), "D' B' D B D R D' R'"},
            { (4,39), "D2 R D' R' D' B' D B"},
            { (4,23), "L D' L' D' F' D F" },
            { (4,27), "D F' D F D L D' L'" },
            { (4,31), "D' B D' B' D' L' D L"},
            { (4,35), "L' D L D B D' B'"},
        };

        public static CubeStats Solve(CubeStats cStats)
        {
            solveEdgesInBottomLayer(cStats);
            cStats.AddToSolution();
            cStats.AddStep(1);
            return cStats;

        }
        // solve edge with id 0
        private static CubeStats SolveEdgeX(CubeStats cStats, int edgeID)
        {
            int pos = SearchBeginner.GetCubieByID(cStats.cube.corners, edgeID);
            string rotation = alignEdgeMoves[(edgeID, pos)];
            Move target = new Move(rotation);
            cStats.cube = target.apply(cStats.cube);
            cStats.Add(rotation);
            return cStats;
        }

        private static CubeStats solveEdgesInBottomLayer(CubeStats cStats)
        {
            int pos;
            string rotation = "";
            for (int i = 8; i < 12; i++)
            {
                pos = SearchBeginner.GetCubieByID(cStats.cube.edges, i);
                if (pos == 7)
                {
                    int cubieFace = int.Parse(SearchBeginner.getCubeString(cStats)[37]);
                    rotation = centerEdgeMoves[(7, cubieFace)];
                }
                if (pos == 6)
                {
                    int cubieFace = int.Parse(SearchBeginner.getCubeString(cStats)[29]);
                    rotation = centerEdgeMoves[(6, cubieFace)];
                }
                if (pos == 5)
                {
                    int cubieFace = int.Parse(SearchBeginner.getCubeString(cStats)[21]);
                    rotation = centerEdgeMoves[(5, cubieFace)];
                }
                if (pos == 4)
                {
                    int cubieFace = int.Parse(SearchBeginner.getCubeString(cStats)[13]);
                    rotation = centerEdgeMoves[(4, cubieFace)];
                }
                Move target = new Move(rotation);
                cStats.cube = target.apply(cStats.cube);
                cStats.Add(rotation);
            }      
            return cStats;
        }

        private static CubeStats CornerFlip(CubeStats cStats, int edgeID)
        {
            int pos = SearchBeginner.GetCubieByID(cStats.cube.edges, edgeID);
            string rotation = "";
            while (cStats.cube.corners[edgeID].orient != 0)
            {
                switch (pos)
                {
                    case 0:
                        rotation = "R' D R D' R' D R"; break;
                    case 1:
                        rotation = "F' D F D' F' D F"; break;
                    case 2:
                        rotation = "L' D L D' L' D L"; break;
                    case 3:
                        rotation = "B' D B D' B' D B"; break;
                }
                Move target = new Move(rotation);
                cStats.cube = target.apply(cStats.cube);
                cStats.Add(rotation);
            }
            return cStats;
        }
    }
}
