using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public Vector3Int CubeletPos;
    public Dictionary<Vector3Int, GameObject> cubeletMap = new();

    [Header("Cubelet Prefab")]
    public GameObject cubeletPrefab;
    [Header("Sticker Prefab")]
    public GameObject stickerPrefab;

    // builds a cube from 27 cubelets centered at origo 0,0,0
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

    //creates a cubelet object and its stickers at a given position x ,y z
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

    //This function asigns a sticker and its color to a corresponding side
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

    //This function creates sticker on top of a cubelet with a seleceted color
    private void CreateSticker(GameObject cubelet, Vector3 normal, Color color, String identifier)
    {
        GameObject sticker = Instantiate(stickerPrefab, cubelet.transform);
        sticker.transform.localScale = new Vector3(0.95f, 0.95f, 0.01f);
        sticker.transform.localPosition = normal * 0.51f;
        sticker.transform.localRotation = Quaternion.LookRotation(-normal);
        sticker.transform.localScale = Vector3.one * 0.9f;
        sticker.transform.name = "Sticker_"+identifier;
        sticker.GetComponent<Renderer>().material.color = color;

        sticker.AddComponent<BoxCollider>();
    }
} 

