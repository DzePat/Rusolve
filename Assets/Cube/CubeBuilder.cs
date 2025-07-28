using System.Collections.Generic;
using UnityEngine;

public class CubeBuilder : MonoBehaviour
{
    public Cubelet Cube;
    public Dictionary<Vector3Int, GameObject> cubeletMap = new();

    void Start()
    {
        BuildCube();
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
                    GameObject cubelet = Cube.CreateCubelet(x, y, z);
                    cubeletMap[pos] = cubelet;
                }
            }
        }
    }
}
