using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public Vector3Int CubeletPos;
    public Dictionary<Vector3Int, GameObject> cubeletMap = new();

    [Header("Cubelet Prefab")]
    public GameObject cubeletPrefab;
    [Header("Sticker Prefab")]
    public GameObject stickerPrefab;

    private List<Vector3Int> staticStickers = new List<Vector3Int>()
    {
        new Vector3Int( 0, 1, 0),
        new Vector3Int( 0,-1, 0),
        new Vector3Int( 1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int( 0, 0, 1),
        new Vector3Int( 0, 0,-1),
    };

    /// <summary>
    /// Builds a cube at x = 0, y = 0, z = 1 from 27 Cubelet objects. 
    /// </summary>
    public void BuildCube()
    {

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector3Int pos = new(x, y, z);
                    GameObject cubelet = CreateCubelet(x, y, z);
                    cubeletMap[pos] = cubelet;
                }
            }
        }
    }

    /// <summary>
    /// Creates a cubelet object in specifided position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns> Cubelet with stickers as children </returns>
    public GameObject CreateCubelet(int x, int y, int z)
    {
        CubeletPos = new Vector3Int(x, y, z);
        GameObject cubelet = Instantiate(cubeletPrefab, transform);
        cubelet.transform.localPosition = CubeletPos;
        cubelet.transform.localRotation = Quaternion.identity;
        cubelet.name = $"Cubelet_{x}_{y}_{z}";
        cubelet.GetComponent<Renderer>().material.color = Color.black;

        AddStickers(cubelet, CubeletPos);
        return cubelet;
    }

    /// <summary>
    /// Asigns a sticker of corresponding color depending on cubelets x,y and z positions.
    /// </summary>
    /// <param name="cubelet"></param>
    /// <param name="position"></param>
    void AddStickers(GameObject cubelet, Vector3Int position)
    {
        if (position.y == 1)
            CreateSticker(cubelet, Vector3.up, Color.white, "white");

        if (position.y == -1)
            CreateSticker(cubelet, Vector3.down, Color.yellow, "yellow");

        if (position.x == -1)
            CreateSticker(cubelet, Vector3.left, Color.orange, "orange");

        if (position.x == 1)
            CreateSticker(cubelet, Vector3.right, Color.red, "red");

        if (position.z == 1)
            CreateSticker(cubelet, Vector3.forward, Color.blue, "blue");

        if (position.z == -1)
            CreateSticker(cubelet, Vector3.back, Color.green, "green");
    }

    /// <summary>
    /// Creates and attaches sticker to a cubelet as a cubelet child. 
    /// </summary>
    /// <param name="cubelet"></param>
    /// <param name="normal"></param>
    /// <param name="color"></param>
    /// <param name="identifier"></param>
    private void CreateSticker(GameObject cubelet, Vector3 normal, Color color, String identifier)
    {
        GameObject sticker = Instantiate(stickerPrefab, cubelet.transform);
        sticker.transform.localScale = new Vector3(0.95f, 0.95f, 0.01f);
        sticker.transform.localPosition = normal * 0.51f;
        sticker.transform.localRotation = Quaternion.LookRotation(-normal);
        sticker.transform.localScale = Vector3.one * 0.9f;
        sticker.transform.name = "Sticker_"+identifier;
        sticker.GetComponent<Renderer>().material.color = color;

        //do not add collision to anchor stickers
        if (!staticStickers.Contains(Vector3Int.RoundToInt(cubelet.transform.position)))
        {
            sticker.AddComponent<BoxCollider>();
        }
    }
} 

