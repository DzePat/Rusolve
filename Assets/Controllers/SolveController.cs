using UnityEngine;
using Kociemba;
using System.Collections.Generic;
using System.Linq;

public class SolveController : MonoBehaviour
{

    public CubeController cubeController;

    Dictionary<string, string> colorToFaceMap = new Dictionary<string, string>()
    {
        { "Sticker_white",  "U" },  // Up
        { "Sticker_yellow", "D" },  // Down
        { "Sticker_green",  "F" },  // Front
        { "Sticker_blue",   "B" },  // Back
        { "Sticker_orange", "L" },  // Left
        { "Sticker_red",    "R" }   // Right
    };

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

    void KociembaSolveAlgorithm()
    {
        string searchString = "";
        List<Vector3Int>[] facesArray = new List<Vector3Int>[]
        {
            SortFace(cubeController.topFace,"U"),
            SortFace(cubeController.rightFace,"R"),
            SortFace(cubeController.frontFace,"F"),
            SortFace(cubeController.bottomFace,"D"),
            SortFace(cubeController.leftFace,"L"),
            SortFace(cubeController.backFace,"B")
        };
              
        foreach (List<Vector3Int> face in facesArray)
        {
            foreach (var sticker in cubeController.GetFaceColors(face))
            {
                string color = sticker.Value;
                searchString += colorToFaceMap[color];
            }
        }
        
        Debug.Log(searchString);

        string info = "";
        string solution = SearchRunTime.solution(searchString, out info, buildTables: true);

        Debug.Log("Solution: " + solution);
        Debug.Log("Solver Info: " + info);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        string scramble = "UUUUUULLLURRURRURRFFFFFFFFFRRRDDDDDDLLDLLDLLDBBBBBBBBB";

        string info = "";
        string solution = SearchRunTime.solution(scramble, out info, buildTables: true);

        Debug.Log("Solution: " + solution);
        Debug.Log("Solver Info: " + info);
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            KociembaSolveAlgorithm();
        }
    }
}
