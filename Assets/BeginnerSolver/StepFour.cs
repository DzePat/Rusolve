using BeginnerSolve;
using System;
using System.Collections.Generic;
using System.Linq;
using TwoPhaseSolver;

namespace Assets.BeginnerSolver
{
    internal class StepFour
    {
        private static readonly Dictionary<(int, int), string> yellowEdgeMoves = new()
        {
                //L shape
                {(41,43), "L B D B' D' B D B' D' L'"},
                {(43,45), "F L D L' D' L D L' D' F'" },
                {(45,47), "R F D F' D' F D F' D' R'" },
                {(47,41), "B R D R' D' R D R' D' B'"},

                //Line shape
                {(41,45), "L B D B' D' L'"},
                {(43,47), "F L D L' D' F'"},

                //Dot shape
                {(-1,-1), "F L D L' D' L D L' D' L D L' D' F'" }
        };

        private static readonly int[] yellowEdges = new int[4] {41,43,45,47};


        private static (int,int) isYellowCrossSolved(CubeStats cStats)
        {
            string[] cube = SearchBeginner.getCubeString(cStats);
            for (int i = 0; i < 4; i++)
            {
                int edgeSticker = int.Parse(cube[yellowEdges[i]]);
                if (!yellowEdges.Contains(edgeSticker))
                {
                    return (-1,-1);
                }
            }
            return (0, 0);
        }

        private static (int,int) isYellowLineSolved(CubeStats cStats)
        {
            string[] cube = SearchBeginner.getCubeString(cStats);
            int a = int.Parse(cube[yellowEdges[0]]);
            int b = int.Parse(cube[yellowEdges[1]]);
            int c = int.Parse(cube[yellowEdges[2]]);
            int d = int.Parse(cube[yellowEdges[3]]);
            if (yellowEdges.Contains(a) && yellowEdges.Contains(c))
            {
                return (41,45);
            }
            else if(yellowEdges.Contains(b) && yellowEdges.Contains(d))
            {
                return (43,47);
            }
            else
            {
                return (0, 0);
            }
        }

        private static (int,int) isYellowLSolved(CubeStats cStats)
        {
            string[] cube = SearchBeginner.getCubeString(cStats);
            int a = int.Parse(cube[yellowEdges[0]]);
            int b = int.Parse(cube[yellowEdges[1]]);
            int c = int.Parse(cube[yellowEdges[2]]);
            int d = int.Parse(cube[yellowEdges[3]]);
            if(yellowEdges.Contains(a) && yellowEdges.Contains(b))
            {
                return (41,43);
            }
            else if(yellowEdges.Contains(b) && yellowEdges.Contains(c))
            {
                return (43,45);
            }
            else if(yellowEdges.Contains(c) && yellowEdges.Contains(d))
            {
                return (45,47);
            }
            else if(yellowEdges.Contains(d) && yellowEdges.Contains(a)){
                return (47,41);
            }
            else
            {
                return (0,0);
            }
        }

        public static CubeStats Solve(CubeStats cStats)
        {
         
            SolveYellowCross(cStats);
            cStats.AddToSolution();
            cStats.AddStep(3);
            return cStats;

        }

        // solve edge with id 0
        private static void SolveYellowCross(CubeStats cStats)
        {
            if (isYellowCrossSolved(cStats) != (0,0)) 
            {
                (int, int) result = isYellowLineSolved(cStats);
                if (result != (0, 0))
                {
                    CheckYellowState(cStats, isYellowLineSolved(cStats));
                }
                else
                {
                    result = isYellowLSolved(cStats);
                    if(result != (0, 0))
                    {
                        CheckYellowState(cStats, isYellowLSolved(cStats));
                    }
                    else
                    {
                        CheckYellowState(cStats, (-1,-1));
                    }
                }
                
            }
        }

        private static void CheckYellowState(CubeStats cStats, (int, int) result)
        {
            if (result != (0, 0))
            {
                string rotation = yellowEdgeMoves[result];
                Move target = new Move(rotation);
                cStats.cube = target.apply(cStats.cube);
                cStats.Add(rotation);
            }
        }
    }
}
