using BeginnerSolve;
using System;
using System.Collections.Generic;
using TwoPhaseSolver;
using UnityEngine;

namespace Assets.BeginnerSolver
{
    public class StepOne
    {
        private static readonly Dictionary<(int, int), string> edgeMoves = new()
        {
                {(0,1), "U'"},
                {(0,2), "U2"},
                {(0,3), "U"},
                {(0,4), "R2"},
                {(0,5), "B R2"},
                {(0,6), "B2 R2"},
                {(0,7), "B' R2"},
                {(0,8), "R"},
                {(0,9), "F U'"},
                {(0,10), "L U2"},
                {(0,11), "R'"},

                {(1,2), "L F"},
                {(1,3), "B2 D2 F2"},
                {(1,4), "D' F2"},
                {(1,5), "F2"},
                {(1,6), "D F2"},
                {(1,7), "D2 F2"},
                {(1,8), "F'"},
                {(1,9), "F"},
                {(1,10), "L2 F"},
                {(1,11), "R D' R' F2"},

                {(2,3), "B L"},
                {(2,4), "D2 L2"},
                {(2,5), "D' L2"},
                {(2,6), "L2"},
                {(2,7), "D L2"},
                {(2,8), "F D' F' L2"},
                {(2,9), "L'"},
                {(2,10), "L"},
                {(2,11), "R D2 R' L2"},

                {(3,4), "D B2"},
                {(3,5), "D2 B2"},
                {(3,6), "D' B2"},
                {(3,7), "B2"},
                {(3,8), "F D2 F' B2"},
                {(3,9), "F' D2 F B2"},
                {(3,10), "B'"},
                {(3,11), "B"},
        };

        public static CubeStats Solve(CubeStats cStats)
        {
            for (int i = 0; i < 4; i++)
            {
                if (cStats.cube.edges[i].pos != i)
                {
                    cStats = SolveEdgeX(cStats,i);
                }
                if (cStats.cube.edges[i].orient != 0)
                {
                    cStats = EdgeFlip(cStats, i);
                }
            }
            cStats.AddToSolution();
            cStats.AddStep(0);
            return cStats;

        }
        // solve edge with id 0
        private static CubeStats SolveEdgeX(CubeStats cStats,int edgeID)
        {
            int pos = SearchBeginner.GetCubieByID(cStats.cube.edges, edgeID);
            string rotation = edgeMoves[(edgeID, pos)];
            Move target = new Move(rotation);
            cStats.cube = target.apply(cStats.cube);
            cStats.Add(rotation);
            return cStats;
        }

        private static CubeStats EdgeFlip(CubeStats cStats, int edgeID)
        {
            int pos = SearchBeginner.GetCubieByID(cStats.cube.edges, edgeID);
            string rotation = "";
            switch (pos)
            {
                case 0:
                    rotation = "R U' B U"; break;
                case 1:
                    rotation = "F U' R U"; break;
                case 2:
                    rotation = "L U' F U"; break;
                case 3:
                    rotation = "B U' L U"; break;
            }
            Move target = new Move(rotation);
            cStats.cube = target.apply(cStats.cube);
            cStats.Add(rotation);
            return cStats;
        }

    }
}
