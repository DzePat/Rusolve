using System.Collections.Generic;
using UnityEngine;

public class CubeBuilder : MonoBehaviour
{
    public Cubelet cubelet;
    private Dictionary<Vector3Int,GameObject> cubeletMap = new Dictionary<Vector3Int, GameObject>();

    
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
                    cubelet.CreateCubelet(x, y, z);
                }
            }
        }
    }
}
