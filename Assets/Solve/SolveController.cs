using System.Collections.Generic;
using TwoPhaseSolver;
using UnityEngine;


public class SolveController : MonoBehaviour
{
    public string[] cubeSolution;
    public SolveManager solveManager;

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
                temp[index] = solveManager.nameToColor[name];
                index++;
            }

            foreach (int i in clockwiseIndexing)
            {
                cubeFacelets[faceletIndex] = temp[i];
                faceletIndex++;
            }
        }

        Cube c = new(cubeFacelets);
        Move pattern = Move.None;

        string solution = Search.patternSolve(c, pattern, 22, printInfo: true).ToString();

        string[] solutionArray = solution.Split(' ');
        return solutionArray;
    }

    private void Start()
    {
        solveManager.SetMoveMap();
    }
}
