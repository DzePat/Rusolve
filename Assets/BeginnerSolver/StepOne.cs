using BeginnerSolve;
using System;
using UnityEngine;
using TwoPhaseSolver;

namespace Assets.BeginnerSolver
{
    public class StepOne
    {
        public static CubeStats Solve(CubeStats cStats)
        {
            CubeStats result = cStats;
            for (int i = 0; i < 4; i++)
            {
                if (result.cube.edges[i].pos != i)
                {
                    result = functions[i](result);
                }
                if (result.cube.edges[i].orient != 0)
                {
                    result = EdgeFlip(result, i);
                }
            }
            result.AddToSolution();
            result.AddStep(0);
            return result;

        }

        // solve edge with id 0
        private static CubeStats SolveEdgeOne(CubeStats cStats)
        {
            Debug.Log("was in step one");
            CubeStats result = cStats;
            int pos = SearchBeginner.GetCubieByID(result.cube.edges, 0);
            string rotation = "";
            switch (pos)
            {
                case 1:
                    rotation = "U'";
                    break;
                case 2:
                    rotation = "U2";
                    break;
                case 3:
                    rotation = "U";
                    break;
                case 4:
                    rotation = "R2";
                    break;
                case 5:
                    rotation = "B R2";
                    break;
                case 6:
                    rotation = "B2 R2";
                    break;
                case 7:
                    rotation = "B' R2";
                    break;
                case 8:
                    rotation = "R";
                    break;
                case 9:
                    rotation = "F U'";
                    break;
                case 10:
                    rotation = "L U2";
                    break;
                case 11:
                    rotation = "R'";
                    break;
            }
            Move target = new Move(rotation);
            result.cube = target.apply(result.cube);
            result.Add(rotation);
            return result;
        }

        //solve edge with id 1
        private static CubeStats SolveEdgeTwo(CubeStats cStats)
        {
            Debug.Log("was in step two");
            CubeStats result = cStats;
            int pos = SearchBeginner.GetCubieByID(result.cube.edges, 1);
            string rotation = "";
            switch (pos)
            {
                case 2:
                    rotation = "L F";
                    break;
                case 3:
                    rotation = "B2 D2 F2";
                    break;
                case 4:
                    rotation = "D' F2";
                    break;
                case 5:
                    rotation = "F2";
                    break;
                case 6:
                    rotation = "D F2";
                    break;
                case 7:
                    rotation = "D2 F2";
                    break;
                case 8:
                    rotation = "F'";
                    break;
                case 9:
                    rotation = "F";
                    break;
                case 10:
                    rotation = "L2 F";
                    break;
                case 11:
                    rotation = "R D' R' F2";
                    break;
            }
            Move target = new Move(rotation);
            result.cube = target.apply(result.cube);
            result.Add(rotation);
            return result;
        }

        // solve edge with id 2
        private static CubeStats SolveEdgeThree(CubeStats cStats)
        {
            Debug.Log("was in step three");
            CubeStats result = cStats;
            int pos = SearchBeginner.GetCubieByID(result.cube.edges, 2);
            string rotation = "";
            switch (pos)
            {
                case 3:
                    rotation = "B L";
                    break;
                case 4:
                    rotation = "D2 L2";
                    break;
                case 5:
                    rotation = "D' L2";
                    break;
                case 6:
                    rotation = "L2";
                    break;
                case 7:
                    rotation = "D L2";
                    break;
                case 8:
                    rotation = "F D' F' L2";
                    break;
                case 9:
                    rotation = "L'";
                    break;
                case 10:
                    rotation = "L";
                    break;
                case 11:
                    rotation = "R D2 R' L2";
                    break;
            }
            Move target = new Move(rotation);
            result.cube = target.apply(result.cube);
            result.Add(rotation);
            return result;
        }

        //solve edge with id 3
        private static CubeStats SolveEdgeFour(CubeStats cStats)
        {
            Debug.Log("was in step four");
            CubeStats result = cStats;
            int pos = SearchBeginner.GetCubieByID(result.cube.edges, 3);
            string rotation = "";
            switch (pos)
            {
                case 4:
                    rotation = "D B2";
                    break;
                case 5:
                    rotation = "D2 B2";
                    break;
                case 6:
                    rotation = "D' B2";
                    break;
                case 7:
                    rotation = "B2";
                    break;
                case 8:
                    rotation = "F D2 F' B2";
                    break;
                case 9:
                    rotation = "F' D2 F B2";
                    break;
                case 10:
                    rotation = "B'";
                    break;
                case 11:
                    rotation = "B";
                    break;
            }
            Move target = new Move(rotation);
            result.cube = target.apply(result.cube);
            result.Add(rotation);
            return result;
        }

        private static CubeStats EdgeFlip(CubeStats cStats, int edgeID)
        {
            CubeStats result = cStats;
            int pos = SearchBeginner.GetCubieByID(result.cube.edges, edgeID);
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
            result.cube = target.apply(result.cube);
            result.Add(rotation);
            return result;
        }

        private static Func<CubeStats,CubeStats>[] functions = { SolveEdgeOne, SolveEdgeTwo, SolveEdgeThree, SolveEdgeFour };
    }
}
