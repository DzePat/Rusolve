using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public Cubelet cube;
    public Dictionary<Vector3Int, GameObject> cubeletMap = new();

    public List<Vector3Int> topFace = GetFace(1,1);
    public List<Vector3Int> bottomFace = GetFace(1, -1);
    public List<Vector3Int> frontFace = GetFace(2, 1);
    public List<Vector3Int> backFace = GetFace(2, -1);
    public List<Vector3Int> rightFace = GetFace(0, 1);
    public List<Vector3Int> leftFace = GetFace(0, -1);

    public static List<Vector3Int> GetFace(int axis,int value)
    {
        List<Vector3Int> face = new List<Vector3Int>();
        int a  =(axis + 1) % 3;
        int b = (axis + 2) % 3;

        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                int[] pos = new int[3];
                pos[axis] = value;
                pos[a] = i;
                pos[b] = j;
                face.Add(new Vector3Int(pos[0], pos[1], pos[2]));
            }
        return face;
    }

    public void RotateFace(List<Vector3Int> face, bool clockwise)
    {
        Dictionary<Vector3Int, GameObject> originalCubletPositions = new();
        foreach (Vector3Int pos in face)
        {
            originalCubletPositions.Add(pos, cubeletMap[pos]);
            cubeletMap.Remove(pos);
        }

        foreach (var cubelet in originalCubletPositions)
        {
            Vector3Int oldPos = cubelet.Key;
            GameObject cubeletObject = cubelet.Value;

            Vector3Int newPos = Vector3Int.RoundToInt(RotateVectorInPlane(oldPos, face[4], clockwise));

            cubeletMap[newPos] = cubeletObject;
            foreach (Transform child in cubeletObject.transform)
            {
                Vector3 stickerOldLocalPos = child.transform.localPosition;
                child.localRotation = Quaternion.AngleAxis(90, face[4]) * child.localRotation;
                child.transform.localPosition = RotateVectorInPlane(stickerOldLocalPos, face[4], clockwise);
            }
            cubeletObject.transform.localPosition = newPos;
        }
    }

    Vector3 RotateVectorInPlane(Vector3 pos, Vector3Int center, bool clockwise)
    {
        Vector3 relative = pos - center;

        Vector3 rotated = center switch
        {
            Vector3Int x when x.x != 0 => new Vector3(relative.x, clockwise ? -relative.z : relative.z, clockwise ? relative.y : -relative.y),
            Vector3Int y when y.y != 0 => new Vector3(clockwise ? relative.z : -relative.z, relative.y, clockwise ? -relative.x : relative.x),
            Vector3Int z when z.z != 0 => new Vector3(clockwise ? -relative.y : relative.y, clockwise ? relative.x : -relative.x, relative.z),
            _ => throw new Exception("Invalid rotation axis")
        };

        return rotated + center;
    }

    void Start()
    {
        BuildCube();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateFace(rightFace, true);
        }
    }

    void BuildCube()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector3Int pos = new(x, y, z);
                    GameObject cubelet = cube.CreateCubelet(x, y, z);
                    cubeletMap[pos] = cubelet;
                }
            }
        }
    }
}
