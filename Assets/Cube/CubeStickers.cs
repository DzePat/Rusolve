using System;
using UnityEditor.Rendering;
using UnityEngine;

public class CubeStickers : MonoBehaviour
{

    [Header("Sticker Prefab")]
    public GameObject stickerPrefab;

    [Header("Sticker Materials")]
    public Material matUp, matDown, matLeft, matRight, matBack, matFront;


    public void AddStickers(GameObject cubelet, Vector3Int position)
    {
        if (position.y == 1)
            CreateSticker(cubelet, Vector3.up, matUp);

        if (position.y == -1)
            CreateSticker(cubelet, Vector3.down, matDown);

        if (position.x == -1)
            CreateSticker(cubelet, Vector3.left, matLeft);

        if (position.x == 1)
            CreateSticker(cubelet, Vector3.right, matRight);

        if (position.z == 1)
            CreateSticker(cubelet, Vector3.forward, matFront);

        if (position.z == -1)
            CreateSticker(cubelet, Vector3.back, matBack);
    }

    public void ChangeColor(GameObject sticker)
    {
        Renderer rend = sticker.GetComponent<Renderer>();
        Material currentMat = rend.sharedMaterial;
        if (currentMat == matUp) {currentMat = matDown;}
        else if (currentMat == matDown) { currentMat = matLeft;}
        else if (currentMat == matLeft) { currentMat = matRight;}
        else if (currentMat == matRight) {  currentMat = matFront;}
        else if (currentMat == matFront) {  currentMat = matBack;}
        else if (currentMat == matBack) {  currentMat = matUp;}
        rend.material = currentMat;
    }

    private void CreateSticker(GameObject cubelet, Vector3 normal, Material mat)
    {
        GameObject sticker = Instantiate(stickerPrefab, cubelet.transform);
        sticker.transform.localScale = new Vector3(0.95f, 0.95f, 0.01f);
        sticker.transform.localPosition = normal * 0.51f; 
        sticker.transform.localRotation = Quaternion.LookRotation(-normal); 
        sticker.transform.localScale = Vector3.one * 0.9f; 
        sticker.GetComponent<Renderer>().material = mat;
        sticker.AddComponent<BoxCollider>();

        StickerClick click = sticker.AddComponent<StickerClick>();
        click.cubeManager = this;
    }
}
