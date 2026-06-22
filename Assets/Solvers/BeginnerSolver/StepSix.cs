using System.Collections.Generic;
using System.Diagnostics;
using TwoPhaseSolver;

namespace Assets.BeginnerSolver
{
    internal class StepSix
    {
        private static readonly Dictionary<int, string> cornerAlignMoves = new()
        {
                {4, "D F D' B' D F' D' B"},
                {5, "D L D' R' D L' D' R"},
                {6, "D B D' F' D B' D' F"},
                {7, "D R D' L' D R' D' L"},
        };

        public static void Solve(CubeStats cStats)
        {
            if (allCornersInPosition(cStats) == false)
            {
                AlignBottomCorners(cStats);
                cStats.AddToSolution();
                cStats.AddStep(5);
            }
        }

        private static int cornerInPosition(CubeStats cStats)
        {
            for (int i = 4; i < 8; i++)
            {
                if(cStats.cube.corners[i].pos == i)
                {
                    return i;
                }
            }
            return 0;
        }

        private static bool allCornersInPosition(CubeStats cStats)
        {
            for (int i = 4; i < 8; i++)
            {
                if (cStats.cube.corners[i].pos != i)
                {
                    return false;
                }
            }
            return true;
        }

        private static void AlignBottomCorners(CubeStats cStats)
        {
            string rotation = "";
            int rightCorner = cornerInPosition(cStats);
            while (rightCorner == 0)
            {
                rotation = cornerAlignMoves[4];
                Move target = new Move(rotation);
                cStats.cube = target.apply(cStats.cube);
                cStats.Add(rotation);
                rightCorner = cornerInPosition(cStats);
            }
            while (allCornersInPosition(cStats) == false)
            {
                rotation = cornerAlignMoves[rightCorner];
                Move target = new Move(rotation);
                cStats.cube = target.apply(cStats.cube);
                cStats.Add(rotation);
                rightCorner = cornerInPosition(cStats);
            }          
        }
    }
}
