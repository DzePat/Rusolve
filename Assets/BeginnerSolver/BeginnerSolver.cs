using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace BeginnerSolve
{
    public static class SearchBeginner
    {
        public static string[] StartSearch(byte[] cubeState)
        {
            string hex = BitConverter.ToString(cubeState).Replace("-", " ");
            Debug.Log(VisualCubeState(hex.Split(" ")));

            return new string[1];
        }

        private static string VisualCubeState(string[] cubeStateString)
        {
            return $"               {cubeStateString[0]} {cubeStateString[1]} {cubeStateString[2]}\n" +
                $"               {cubeStateString[7]}      {cubeStateString[3]}\n" +
                $"               {cubeStateString[6]} {cubeStateString[5]} {cubeStateString[4]}\n" +
                $"{cubeStateString[24]} {cubeStateString[25]} {cubeStateString[26]}{cubeStateString[16]} {cubeStateString[17]} {cubeStateString[18]}{cubeStateString[8]} {cubeStateString[9]} {cubeStateString[10]}{cubeStateString[32]} {cubeStateString[33]} {cubeStateString[34]}\n" +
                $"{cubeStateString[31]}      {cubeStateString[27]}{cubeStateString[23]}      {cubeStateString[19]}{cubeStateString[15]}      {cubeStateString[11]}{cubeStateString[39]}      {cubeStateString[35]}\n" +
                $"{cubeStateString[30]} {cubeStateString[29]} {cubeStateString[28]}{cubeStateString[22]} {cubeStateString[21]} {cubeStateString[20]}{cubeStateString[14]} {cubeStateString[13]} {cubeStateString[12]}{cubeStateString[38]} {cubeStateString[37]} {cubeStateString[36]}\n" +
                $"               {cubeStateString[40]} {cubeStateString[41]} {cubeStateString[42]}\n" +
                $"               {cubeStateString[47]}       {cubeStateString[43]}\n" +
                $"               {cubeStateString[46]} {cubeStateString[45]} {cubeStateString[44]}\n";


        }

    } 
}

