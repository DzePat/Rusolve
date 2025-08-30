using System;
using System.Collections.Generic;
using System.Linq;
using TwoPhaseSolver;
using UnityEngine;
using UnityEngine.Windows;

public class SolveController : MonoBehaviour
{

    public CubeController cubeController;

    Dictionary<string, byte> nameToColor = new Dictionary<string, byte>()
    {
        { "Sticker_white",  0 },  // Up
        { "Sticker_red",    1 },  // Right
        { "Sticker_green",  2 },  // Front
        { "Sticker_orange", 3 },  // Left
        { "Sticker_blue",   4 },  // Back
        { "Sticker_yellow", 5 }  // Down
    };

    public Dictionary<char, List<Vector3Int>> moveMap = new Dictionary<char, List<Vector3Int>>();


    List<Vector3Int> SortFace(List<Vector3Int> face, string faceName)
    {
        switch (faceName)
        {
            case "U": // Top face (Y = +1)
                return face.OrderByDescending(p => p.z).ThenBy(p => p.x).ToList();

            case "D": // Bottom face (Y = -1)
                return face.OrderBy(p => p.z).ThenBy(p => p.x).ToList();

            case "F": // Front face 
                return face.OrderByDescending(p => p.y).ThenBy(p => p.x).ToList();

            case "B": // Back face 
                return face.OrderByDescending(p => p.y).ThenByDescending(p => p.x).ToList();

            case "L": // Left face (X = -1)
                return face.OrderByDescending(p => p.y).ThenByDescending(p => p.z).ToList();

            case "R": // Right face (X = +1)
                return face.OrderByDescending(p => p.y).ThenBy(p => p.z).ToList();

            default:
                Debug.LogWarning($"Unknown face name: {faceName}");
                return face;
        }
    }

    public string[] KociembaSolveAlgorithm()
    {
        string searchString = "";
        List<Vector3Int>[] facesArray = new List<Vector3Int>[]
        {
            SortFace(cubeController.topFace,"U"),
            SortFace(cubeController.rightFace,"R"),
            SortFace(cubeController.frontFace,"F"),
            SortFace(cubeController.leftFace,"L"),
            SortFace(cubeController.backFace,"B"),
            SortFace(cubeController.bottomFace,"D")
        };

        int[] clockwiseIndexing = new int[] {0, 1, 2, 5, 8, 7, 6, 3};
        byte[] cubeFacelets = new byte[48];
        int faceletIndex = 0;
        foreach (List<Vector3Int> face in facesArray)
        {
            byte[] temp = new byte[9];
            int index = 0;
            foreach (var sticker in cubeController.GetFaceColors(face))
            {
                    string name = sticker.Value;
                    temp[index]= nameToColor[name];
                    index++;
            }

            foreach(int i in clockwiseIndexing)
            {
                cubeFacelets[faceletIndex] = temp[i];
                faceletIndex++;
            }
        }

        Cube c = new Cube(cubeFacelets);
        Move pattern = Move.None;
        string solution = Search.patternSolve(c, pattern, 22, printInfo: true).ToString();
        string[] solutionArray = solution.Split(' ');
        return solutionArray;
    }

    public void populateMoveMap()
    {
        moveMap.Add(key: 'R', value: cubeController.rightFace);
        moveMap.Add(key: 'L', value: cubeController.leftFace);
        moveMap.Add(key: 'U', value: cubeController.topFace);
        moveMap.Add(key: 'D', value: cubeController.bottomFace);
        moveMap.Add(key: 'B', value: cubeController.backFace);
        moveMap.Add(key: 'F', value: cubeController.frontFace);
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
}
