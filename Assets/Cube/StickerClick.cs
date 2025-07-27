using System;
using UnityEngine;

public class StickerClick : MonoBehaviour
{
    public Cubelet parentCubelet; 

    void OnMouseDown()
    {
        parentCubelet.ChangeColor(gameObject);
    }
}