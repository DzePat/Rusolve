using Assets.BeginnerSolver;
using System;
using System.Linq;
using TwoPhaseSolver;
using UnityEngine;

namespace BeginnerSolve
{
    public static class SearchBeginner
    {
        public static string[] StartSearch(Cube cubeState)
        {

            CubeStats cubeStats = new CubeStats(cubeState);
            StepOne.Solve(cubeStats);
            StepTwo.Solve(cubeStats);
            StepThree.Solve(cubeStats);
            StepFour.Solve(cubeStats);
            StepFive.Solve(cubeStats);
            return cubeStats.solution.Split(" ");
        }

        public static string[] getCubeString(CubeStats cubeStats)
        {
            string hex = BitConverter.ToString(cubeStats.cube.getFacelets()).Replace("-", " ");
            string[] hexArr = hex.Split(" ");
            int[] decimalValues = hexArr.Select(h => Convert.ToInt32(h, 16)).ToArray();
            string[] cubeStateString = decimalValues.Select(d => d.ToString("D2")).ToArray();
            return cubeStateString;
        }

        //check if white cross is solved
        public static bool IsStepSolved(Cubie[] cubieList,int rangeFrom, int rangeTo)
        {
            for (int i = rangeFrom; i < rangeTo; i++)
            {
                if (cubieList[i].pos != i || cubieList[i].orient != 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Takes cubieList and cubieID as an argument. returns the position index of where the cubie has been found.
        /// </summary>
        /// <param name="cubieList"></param>
        /// <param name="cubieID"></param>
        /// <returns>Cubie index in the list</returns>
        public static int GetCubieByID(Cubie[] cubieList, int cubieID)
        {
            int i = 0;
            foreach(Cubie c in cubieList) {
                if(c.pos == cubieID)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }


    } 
}

