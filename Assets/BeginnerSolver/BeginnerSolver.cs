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
            cubeStats = StepOne(cubeStats);
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
        private static bool IsStepSolved(Cubie[] cubieList)
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
        private static int GetCubieByID(Cubie[] cubieList, int cubieID)
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

        /// <summary>
        /// Takes EdgeID as argument. returns cubeState with edge solved and rotation sequence in results.
        /// </summary>
        /// <param name="cStats"></param>
        /// <param name="edgeID"></param>
        /// <returns> cubeState with solved edge</returns>
        private static CubeStats SolveCrossEdge(CubeStats cStats,int edgeID)
        {

            CubeStats result = cStats;
            for (int j = edgeID; j > 0; j--)
            {
                Move target = new Move("U'");
                result.cube = target.apply(result.cube);
                result.Add("U'");
            }
            int pos = GetCubieByID(result.cube.edges, edgeID);
            if (pos == 1)
            {
                Move F = new Move("F");
                result.cube = F.apply(result.cube);
                result.Add("F");
                pos = GetCubieByID(result.cube.edges, edgeID);
            }
            if (pos == 3)
            {
                Move B = new Move("B'");
                result.cube = B.apply(result.cube);
                result.Add("B'");
                pos = GetCubieByID(result.cube.edges, edgeID);
            }
            if (pos == 2)
            {
                Move L = new Move("L");
                result.cube = L.apply(result.cube);
                result.Add("L");
                pos = GetCubieByID(result.cube.edges, edgeID);
            }
            if(pos == 5 ||pos == 7)
            {
                Move D = new Move("D");
                result.cube = D.apply(result.cube);
                result.Add("D");
                pos = GetCubieByID(result.cube.edges, edgeID);
            }
            if (pos == 6 || pos == 9 || pos == 10)
            {
                Move U = new Move("U2");
                result.cube = U.apply(result.cube);
                result.Add("U2");
                pos = GetCubieByID(result.cube.edges, edgeID);
                Move L = new Move("L");
                while (pos != 2)
                {
                    Debug.Log("stuck in pos 2 loop");
                    result.cube = L.apply(result.cube);
                    result.Add("L");
                    pos = GetCubieByID(result.cube.edges, edgeID);
                }
                result.cube = U.apply(result.cube);
                result.Add("U2");
                pos = GetCubieByID(result.cube.edges, edgeID);
            }
            if (pos == 4 || pos == 8 || pos == 11)
            {
                Move R = new Move("R");
                while (pos != 0)
                {
                    Debug.Log("stuck in pos 0 loop");
                    result.cube = R.apply(result.cube);
                    result.Add("R");
                    pos = GetCubieByID(result.cube.edges, edgeID);

                }
            }
            for (int j = edgeID; j > 0; j--)
            {
                Move target = new Move("U");
                result.cube = target.apply(result.cube);
                result.Add("U");
            }
            return result;
        }

        /// <summary>
        /// takes CubeState as argument, returns cubeState with solved edges.
        /// </summary>
        /// <param name="cStats"></param>
        /// <returns></returns>
        private static CubeStats StepOne(CubeStats cStats) {
            CubeStats result = cStats;
            if(IsStepSolved(cStats.cube.edges) == false)
            {
                for(int i = 0; i < 4; i++)
                {
                    if (result.cube.edges[i].pos != i)
                    {
                        result = SolveCrossEdge(result,i);

                    }else if(result.cube.edges[i].orient != 0)
                    {
                        for(int j = i;  j > 0; j--)
                        {
                            Move target = new Move("U'");
                            result.cube = target.apply(result.cube);
                            result.Add("U'");
                        }
                        Debug.Log($"Flip executed at i={i}");
                        Move Flip = new Move("R U' B U");
                        result.cube = Flip.apply(result.cube);
                        result.Add("R U' B U");
                        for (int j = i; j > 0; j--)
                        {
                            Move target = new Move("U");
                            result.cube = target.apply(result.cube);
                            result.Add("U");
                        }
                    }
                }
                return result;
            }
            else
            {
                return result;
            }
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

