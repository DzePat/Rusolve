using BeginnerSolve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPhaseSolver;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;

namespace Assets.BeginnerSolver
{
    internal class StepSeven
    {
        private static readonly int[] yellowCorners = new int[4] { 40, 42, 44, 46 };

        private static void SolveYellowCorners(CubeStats cStats)
        {
            string rotation = "";
            for(int i  = 0; i < 4; i++)
            {
                string[] cube = SearchBeginner.getCubeString(cStats);
                int corner = int.Parse(cube[40]);
                Move target;
                while (corner != yellowCorners[i])
                {
                    rotation = "L' U' L U";
                    target = new Move(rotation);
                    cStats.cube = target.apply(cStats.cube);
                    cStats.Add(rotation);
                    cube = SearchBeginner.getCubeString(cStats);
                    corner = int.Parse(cube[40]);
                }
                rotation = "D'";
                target = new Move(rotation);
                cStats.cube = target.apply(cStats.cube);
                cStats.Add(rotation);
            }

        }

        private static void AdjustLayers(CubeStats cStats)
        {
            while (cStats.cube.edges[0].pos != 0) 
            {
                Move target = new Move("U");
                cStats.cube = target.apply(cStats.cube);
                cStats.Add("U");
            }
            while (cStats.cube.edges[4].pos != 4)
            {
                Move target = new Move("D");
                cStats.cube = target.apply(cStats.cube);
                cStats.Add("D");
            }

        }

        public static void Solve(CubeStats cStats)
        {
            if (SearchBeginner.IsStepSolved(cStats.cube.corners, 4, 8) == false)
            {
                SolveYellowCorners(cStats);
                AdjustLayers(cStats);
                cStats.AddToSolution();
                cStats.AddStep(6);
            }
        }
    }
}
