using System;
using System.Linq;
using TwoPhaseSolver;
using UnityEngine;

namespace BeginnerSolve
{
    public static class SearchBeginner
    {
        public static string[] StartSearch(TwoPhaseSolver.Cube cubeState)
        {
            string hex = BitConverter.ToString(cubeState.getFacelets()).Replace("-", " ");
            string[] hexArr = hex.Split(" ");
            int[] decimalValues = hexArr.Select(h => Convert.ToInt32(h, 16)).ToArray();
            string[] cubeStateString = decimalValues.Select(d => d.ToString("D2")).ToArray();

            Debug.Log(VisualCubeState(cubeStateString));

            return new string[1];
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

        private static string SolveCross(TwoPhaseSolver.Cube cubeState) {


            return "";
        }



    } 
}

