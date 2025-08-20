using System;
using UnityEngine;

public class StickerClick : MonoBehaviour
{
    public CubeManager parentCubelet; 

    void OnMouseDown()
    {
        parentCubelet.ChangeColor(gameObject);
    }
}