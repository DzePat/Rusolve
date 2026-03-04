using BeginnerSolve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPhaseSolver;

namespace Assets.BeginnerSolver
{
    internal class StepTwo
    {
        private static readonly Dictionary<(int, int), string> cornerMoves = new()
        {
                {(0,1), "L R' D L' R"},
                {(0,2), "L' R' D2 L R"},
                {(0,3), "F B' D' F' B"},
                {(0,4), "D' R' D R"},
                {(0,5), "R' D R"},
                {(0,6), "R' D2 R"},
                {(0,7), "D R' D2 R"},

                {(1,2), "B F' D B' F"},
                {(1,3), "B' L D2 L'"},
                {(1,4), "L D' L'"},
                {(1,5), "D L D' L'"},
                {(1,6), "D' L D2 l'"},
                {(1,7), "L D2 L'"},

                {(2,3), "R L' D R' L"},
                {(2,4), "L' D2 L"},
                {(2,5), "D L' D2 L"},
                {(2,6), "D' L' D L"},
                {(2,7), "L' D L"},

                {(3,4), "D' R D2 R'"},
                {(3,5), "R D2 R'"},
                {(3,6), "R D' R'"},
                {(3,7), "D R D' R'"},
        };

        public static CubeStats Solve(CubeStats cStats)
        {
            for (int i = 0; i < 4; i++)
            {
                if (cStats.cube.corners[i].pos != i)
                {
                    cStats = SolveCornerX(cStats, i);
                }
                if (cStats.cube.corners[i].orient != 0)
                {
                    cStats = CornerFlip(cStats, i);
                }
            }
            cStats.AddToSolution();
            cStats.AddStep(1);
            return cStats;

        }
        // solve edge with id 0
        private static CubeStats SolveCornerX(CubeStats cStats, int edgeID)
        {
            int pos = SearchBeginner.GetCubieByID(cStats.cube.corners, edgeID);
            string rotation = cornerMoves[(edgeID, pos)];
            Move target = new Move(rotation);
            cStats.cube = target.apply(cStats.cube);
            cStats.Add(rotation);
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
