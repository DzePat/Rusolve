using System.Collections.Generic;
using UnityEngine;

public class Cubelet : MonoBehaviour
{
    public Vector3Int CubeletPos;

    [Header("Cubelet Prefab")]
    public GameObject cubeletPrefab;
    [Header("Sticker Prefab")]
    public GameObject stickerPrefab;

    public GameObject CreateCubelet(int x, int y, int z)
    {
        CubeletPos = new Vector3Int(x, y, z);
        GameObject cubelet = Instantiate(cubeletPrefab, transform);
        cubelet.transform.localPosition = CubeletPos;
        cubelet.transform.localRotation = Quaternion.identity;
        cubelet.transform.localScale = Vector3.one;
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
        Renderer rend = sticker.GetComponent<Renderer>();
        Color currentMat = rend.sharedMaterial.color;
        if (currentMat == Color.white) { currentMat = Color.yellow; }
        else if (currentMat == Color.yellow) { currentMat = Color.orange; }
        else if (currentMat == Color.orange) { currentMat = Color.red; }
        else if (currentMat == Color.red) { currentMat = Color.green; }
        else if (currentMat == Color.green) { currentMat = Color.blue; }
        else if (currentMat == Color.blue) { currentMat = Color.white; }
        rend.material.color = currentMat;
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

