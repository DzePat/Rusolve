using BeginnerSolve;
using System.Collections.Generic;
using TwoPhaseSolver;
using UnityEngine;


public class SolveController : MonoBehaviour
{
    public string[] cubeSolution;
    public SolveManager solveManager;
    readonly Dictionary<string, byte> nameToColor = new()
    {
        { "Sticker_white",  0 },  // Up
        { "Sticker_red",    1 },  // Right
        { "Sticker_green",  2 },  // Front
        { "Sticker_orange", 3 },  // Left
        { "Sticker_blue",   4 },  // Back
        { "Sticker_yellow", 5 }  // Down
    };


    /// <summary>
    /// Parses current cube state into a byte array which is in a clockwise order indexing 
    /// Face example with indexes of array:
    ///  +---------+
    ///  | 0  1  2 |
    ///  | 7     3 |
    ///  | 6  5  4 |
    ///  +---------+
    /// </summary>
    /// <returns></returns>
    public string[] GetKociembaSolution()
    {
        byte[] cubeFacelets = getFacelets();

        Cube c = new(cubeFacelets);
        Move pattern = Move.None;

        string solution = Search.patternSolve(c, pattern, 22, printInfo: true).ToString();

        string[] solutionArray = solution.Split(' ');
        return solutionArray;
    }

    public string[] GetBeginnerSolution()
    {
        byte[] cubeFacelets = getFacelets();
        Cube c = new(cubeFacelets);
        string[] solution = SearchBeginner.StartSearch(c);
        return solution;
    }

    private byte[] getFacelets()
    {
        List<Vector3Int>[] facesArray = new List<Vector3Int>[]
                {
            solveManager.SortFace(solveManager.topFace,"U"),
            solveManager.SortFace(solveManager.rightFace,"R"),
            solveManager.SortFace(solveManager.frontFace,"F"),
            solveManager.SortFace(solveManager.leftFace,"L"),
            solveManager.SortFace(solveManager.backFace,"B"),
            solveManager.SortFace(solveManager.bottomFace,"D")
                };

        int[] clockwiseIndexing = new int[] { 0, 1, 2, 5, 8, 7, 6, 3 };
        byte[] cubeFacelets = new byte[48];
        int faceletIndex = 0;
        foreach (List<Vector3Int> face in facesArray)
        {
            byte[] temp = new byte[9];
            int index = 0;
            foreach (var sticker in solveManager.GetFaceColors(face))
            {
                string name = sticker.Value;
                temp[index] = nameToColor[name];
                index++;
            }

            foreach (int i in clockwiseIndexing)
            {
                cubeFacelets[faceletIndex] = temp[i];
                faceletIndex++;
            }
        }

        return cubeFacelets;
    }

    private void Start()
    {
        solveManager.SetMoveMap();
    }
}
