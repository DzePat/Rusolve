using BeginnerSolve;
using System.Collections.Generic;
using TwoPhaseSolver;

namespace Assets.BeginnerSolver
{
    internal class StepFive
    {
        private static readonly Dictionary<(int, int), string> edgeMoves = new()
        {
                {(4,5), "D'"},
                {(4,6), "D2"},
                {(4,7), "D'"},

                {(5,6), "B D B' D B D2 B' D"},
                {(5,7), "R D R' D R D2 R' D B D B' D B D2 B' D"},

                {(6,7), "R D R' D R D2 R' D"},
        };

        public static void Solve(CubeStats cStats)
        {
            if (SearchBeginner.IsStepSolved(cStats.cube.edges, 4, 8) == false)
            {
                for (int i = 4; i < 8; i++)
                {
                    if (cStats.cube.edges[i].pos != i)
                    {
                        SolveEdgeX(cStats, i);
                    }
                }
                cStats.AddToSolution();
                cStats.AddStep(4);
            }
        }
        // solve edge with id 0
        private static void SolveEdgeX(CubeStats cStats, int edgeID)
        {
            int pos = SearchBeginner.GetCubieByID(cStats.cube.edges, edgeID);
            string rotation = edgeMoves[(edgeID, pos)];
            Move target = new Move(rotation);
            cStats.cube = target.apply(cStats.cube);
            cStats.Add(rotation);
        }

    }
}
