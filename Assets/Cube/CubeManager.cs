﻿using System;
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

    void AddStickers(GameObject cubelet, Vector3Int position)
    {
        if (position.y == 1)
            CreateSticker(cubelet, Vector3.up, Color.white);

        if (position.y == -1)
            CreateSticker(cubelet, Vector3.down, Color.yellow);

        if (position.x == -1)
            CreateSticker(cubelet, Vector3.left, Color.orange);

        if (position.x == 1)
            CreateSticker(cubelet, Vector3.right, Color.red);

        if (position.z == 1)
            CreateSticker(cubelet, Vector3.forward, Color.green);

        if (position.z == -1)
            CreateSticker(cubelet, Vector3.back, Color.blue);
    }

    public void ChangeColor(GameObject sticker)
    {
        Color[] colors = new Color[] {Color.white,Color.yellow, Color.orange,Color.red,Color.green,Color.blue};
        Renderer rend = sticker.GetComponent<Renderer>();
        Color currentMat = rend.sharedMaterial.color;
        int index = Array.IndexOf(colors, currentMat);
        index++;
        if (index == colors.Length) index = 0;
        rend.material.color = colors[index];
    }

    private void CreateSticker(GameObject cubelet, Vector3 normal, Color color)
    {
        GameObject sticker = Instantiate(stickerPrefab, cubelet.transform);
        sticker.transform.localScale = new Vector3(0.95f, 0.95f, 0.01f);
        sticker.transform.localPosition = normal * 0.51f;
        sticker.transform.localRotation = Quaternion.LookRotation(-normal);
        sticker.transform.localScale = Vector3.one * 0.9f;
        sticker.GetComponent<Renderer>().material.color = color;

        sticker.AddComponent<BoxCollider>();
        var click = sticker.AddComponent<StickerClick>();
        click.parentCubelet = this;
    }
} 

