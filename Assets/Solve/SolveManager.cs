using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwoPhaseSolver;
using UnityEngine;

public class SolveManager : MonoBehaviour
{
    public CubeController cubeController;

    public readonly Queue<(List<Vector3Int> face, bool clockwise)> rotationQueue = new();
    public bool isRotating = false;
    public Dictionary<char, List<Vector3Int>> moveMap = new();

    public List<Vector3Int> topFace = GetFacePositions(1, 1);
    public List<Vector3Int> bottomFace = GetFacePositions(1, -1);
    public List<Vector3Int> backFace = GetFacePositions(2, 1);
    public List<Vector3Int> frontFace = GetFacePositions(2, -1);
    public List<Vector3Int> rightFace = GetFacePositions(0, 1);
    public List<Vector3Int> leftFace = GetFacePositions(0, -1);

    public readonly Dictionary<string, byte> nameToColor = new()
    {
        { "Sticker_white",  0 },  // Up
        { "Sticker_red",    1 },  // Right
        { "Sticker_green",  2 },  // Front
        { "Sticker_orange", 3 },  // Left
        { "Sticker_blue",   4 },  // Back
        { "Sticker_yellow", 5 }  // Down
    };




    /// <summary>
    /// Populate dictionary with key and its corresponding face example R = right face of the cube
    /// </summary>
    public void SetMoveMap()
    {
        moveMap.Add(key: 'R', value: rightFace);
        moveMap.Add(key: 'L', value: leftFace);
        moveMap.Add(key: 'U', value: topFace);
        moveMap.Add(key: 'D', value: bottomFace);
        moveMap.Add(key: 'B', value: backFace);
        moveMap.Add(key: 'F', value: frontFace);
    }

    /// <summary>
    /// Takes and axis x = 0 , y = 1 , z = 2 and a value 1 or -1 and returns a list of positions of face cubelets
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="value"></param>
    /// <returns> returns a list with 9 face cubelet positions</returns>
    public static List<Vector3Int> GetFacePositions(int axis, int value)
    {
        List<Vector3Int> face = new();
        int a = (axis + 1) % 3;
        int b = (axis + 2) % 3;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int[] pos = new int[3];
                pos[axis] = value;
                pos[a] = i;
                pos[b] = j;
                face.Add(new Vector3Int(pos[0], pos[1], pos[2]));
            }
        }
        return face;
    }

    /// <summary>
    /// Takes a face as argument and returns a list of sticker positions and their corresponding colors
    /// </summary>
    /// <param name="face"></param>
    /// <returns>list of 9 sticker positions and their colors</returns>
    public Dictionary<Vector3Int, string> GetFaceColors(List<Vector3Int> face)
    {
        Vector3Int center = face[4];
        Dictionary<Vector3Int, string> FaceColors = new();

        //get vector identifier x ,y or z
        int position = Mathf.Abs(center.x) == 1 ? 0 : Mathf.Abs(center.y) == 1 ? 1 : 2;

        //vector value to compare against cubelet children
        int value = position == 0 ? center.x : position == 1 ? center.y : center.z;

        //change compare value to 2 because the offset for sticker values are 0.51 and after rounding up its 2.
        float compareValue = value >= 1 ? 2 : -2;

        int childcount = 0;
        //iterate trough 9 face cubelets
        foreach (Vector3Int pos in face)
        {
            GameObject cubelet = cubeController.cubeManager.cubeletMap[pos];
            childcount++;

            //iterate trough cubelet children
            foreach (Transform child in cubeController.cubeManager.cubeletMap[pos].transform)
            {
                Vector3 stickerPos = child.position;

                //round compare axis to get more accurate compare value of 2 or -2
                int comparePos = Mathf.RoundToInt(
                    position == 0 ? stickerPos.x :
                    position == 1 ? stickerPos.y :
                    stickerPos.z
                );

                //find the sticker facing the right direction
                if (comparePos == compareValue)
                {
                    string stickerColor = child.transform.name;
                    FaceColors.Add(pos, stickerColor);
                }
            }
        }

        return FaceColors;
    }

    /// <summary>
    /// Sorts a face 
    /// Example face:
    ///  +---------+
    ///  | 0  1  2 |
    ///  | 3  4  5 |
    ///  | 6  7  8 |
    ///  +---------+
    /// </summary>
    /// <param name="face"></param>
    /// <param name="faceName"></param>
    /// <returns></returns>
    public List<Vector3Int> SortFace(List<Vector3Int> face, string faceName)
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

    /// <summary>
    /// Adds a rotation to the queue
    /// </summary>
    /// <param name="face"></param>
    /// <param name="clockwise"></param>
    public void EnqueueRotation(List<Vector3Int> face, bool clockwise)
    {
        rotationQueue.Enqueue((face, clockwise));

        if (!isRotating)
            StartCoroutine(ProcessQueue());
    }

    /// <summary>
    /// Dequeues enqueued rotations
    /// </summary>
    private IEnumerator ProcessQueue()
    {
        isRotating = true;

        while (rotationQueue.Count > 0)
        {
            var (face, clockwise) = rotationQueue.Dequeue();
            yield return StartCoroutine(cubeController.RotateFace(face, clockwise));
        }

        isRotating = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EnqueueRotation(frontFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            EnqueueRotation(leftFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            EnqueueRotation(rightFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            EnqueueRotation(topFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            EnqueueRotation(bottomFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            EnqueueRotation(backFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {

            Dictionary<Vector3Int, string> FaceColors = GetFaceColors(topFace);
            foreach (var faceColor in FaceColors)
            {
                Debug.Log($"Position: {faceColor.Key} Color: {faceColor.Value}");
            }
        }
    }
}