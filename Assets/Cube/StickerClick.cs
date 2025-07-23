using System;
using UnityEngine;

public class StickerClick : MonoBehaviour
{
    public CubeStickers cubeManager; 

    void OnMouseDown()
    {
        cubeManager.ChangeColor(gameObject);
    }
}