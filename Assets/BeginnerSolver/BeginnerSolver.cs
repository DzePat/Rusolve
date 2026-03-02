using Assets.BeginnerSolver;
using System;
using TwoPhaseSolver;
using UnityEngine;

namespace BeginnerSolve
{
    public static class SearchBeginner
    {
        public static string[] StartSearch(Cube cubeState)
        {

            CubeStats cubeStats = new CubeStats(cubeState);
            if(IsStepSolved(cubeState.edges) == false)
            {
                cubeStats = StepOne.Solve(cubeStats);
            }
            /*string hex = BitConverter.ToString(cubeStats.cube.getFacelets()).Replace("-", " ");
            string[] hexArr = hex.Split(" ");
            int[] decimalValues = hexArr.Select(h => Convert.ToInt32(h, 16)).ToArray();
            string[] cubeStateString = decimalValues.Select(d => d.ToString("D2")).ToArray();
            Debug.Log(VisualCubeState(cubeStateString));*/
            Debug.Log($"stepRotations: {cubeStats.stepRotations}");
            return cubeStats.stepRotations.Split(" ");
        }

        private static string Cell(string v) => $"{v,2}";

        private static string VisualCubeState(string[] c)
        {
            return
                $@"
                {" ",22}|{Cell(c[0])}|{Cell(c[1])}|{Cell(c[2])}|
                {" ",22}|{Cell(c[7])}|{Cell("TT")}|{Cell(c[3])}|
                {" ",22}|{Cell(c[6])}|{Cell(c[5])}|{Cell(c[4])}|
                ----------------------------------------------------
                |{Cell(c[24])}|{Cell(c[25])}|{Cell(c[26])}|-|{Cell(c[16])}|{Cell(c[17])}|{Cell(c[18])}|-|{Cell(c[8])}|{Cell(c[9])}|{Cell(c[10])}|-|{Cell(c[32])}|{Cell(c[33])}|{Cell(c[34])}|
                |{Cell(c[31])}|{Cell("LL")}|{Cell(c[27])}|-|{Cell(c[23])}|{Cell("FF")}|{Cell(c[19])}|-|{Cell(c[15])}|{Cell("RR")}|{Cell(c[11])}|-|{Cell(c[39])}|{Cell("BB")}|{Cell(c[35])}|
                |{Cell(c[30])}|{Cell(c[29])}|{Cell(c[28])}|-|{Cell(c[22])}|{Cell(c[21])}|{Cell(c[20])}|-|{Cell(c[14])}|{Cell(c[13])}|{Cell(c[12])}|-|{Cell(c[38])}|{Cell(c[37])}|{Cell(c[36])}|
                ----------------------------------------------------
                {" ",22}|{Cell(c[40])}|{Cell(c[41])}|{Cell(c[42])}|
                {" ",22}|{Cell(c[47])}|{Cell("DD")}|{Cell(c[43])}|
                {" ",22}|{Cell(c[46])}|{Cell(c[45])}|{Cell(c[44])}|";
        }

        //check if white cross is solved
        public static bool IsStepSolved(Cubie[] cubieList)
        {
            for (int i = 0; i < 4; i++)
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

        //solve white corners for step two
        private static CubeStats StepTwo(CubeStats cStats)
        {
            CubeStats result = cStats;
            if (IsStepSolved(cStats.cube.corners) == false)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (result.cube.corners[i].pos != i)
                    {
                        result = solveWhiteCorner(result, i);

                    }
                }
            }

            return cStats;
        }

        private static CubeStats solveWhiteCorner(CubeStats result, int i)
        {
            throw new NotImplementedException();
        }
    } 
}

