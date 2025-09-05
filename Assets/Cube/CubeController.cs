using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{

    public CubeManager cubeManager;
    public Dictionary<string, Color> colors = new()
{
        { "white", Color.white },
        { "red", Color.red },
        { "green", Color.green },
        { "blue", Color.blue },
        { "yellow",Color.yellow },
        { "orange",Color.orange },
        { "temp",new Color(5f, 0f, 5f)}
    };


    /// <summary>
    /// Utility function for cube layers. Currently not used but will be used for beginner solver steps.
    /// </summary>
    /// <param name="layer"></param>
    /// <returns> returns an array of cubelet positions and their corersponding names</returns>
    public (Vector3Int pos, string name)[] GetCubeletLayers(string layer)
    {
        (Vector3Int pos, string name)[] layerArr = new (Vector3Int, string)[9];
        int layerPosY = layer == "top" ? 1 : layer == "bottom" ? -1 : 0;
        int layerIndex = 0;

        foreach(var  cubelet in cubeManager.cubeletMap)
        {
            if(cubelet.Key.y == layerPosY)
            {
                layerArr[layerIndex] = (cubelet.Key, cubelet.Value.name);
                layerIndex++;
            }
        }

        return layerArr;
    }

    /// <summary>
    /// disables sticker events by removing collider from sticker gameobject.
    /// </summary>
    public void DisableStickerClick()
    {
        foreach(GameObject cubelet in cubeManager.cubeletMap.Values)
        {
            foreach(Transform child in cubelet.transform)
            {
                var collider = child.GetComponent<Collider>();
                Destroy(collider);
            }
        }
    }

    /// <summary>
    /// Rotates a cube face by taking a face as the argument and a boolean which determines clockwise or anticlockwise rotation
    /// </summary>
    /// <param name="face"></param>
    /// <param name="clockwise"></param>
    /// <returns></returns>
    public IEnumerator RotateFace(List<Vector3Int> face, bool clockwise)
    {
        Vector3 center = face[4];

        //temporary parent Gameobject for the 9 cubelets thats gonna rotate
        GameObject pivot = new("Pivot");
        
        //set pivot point for the game object
        pivot.transform.position = face[4];

        //9 Cubelets for the rotation side
        Dictionary<Vector3Int, GameObject> originalCubletPositions = new();

        //get all 9 cubelets for the side to rotate from the main cubelet Dict
        foreach (Vector3Int pos in face)
        {
            originalCubletPositions.Add(pos, cubeManager.cubeletMap[pos]);
            cubeManager.cubeletMap.Remove(pos);
        }

        //asign all 9 cubelets to the pivot gameobject
        foreach (GameObject cubelet in originalCubletPositions.Values)
            cubelet.transform.SetParent(pivot.transform);

        // set clockwise anti clockwise values for rotation
        float RotationAngle = clockwise ? 90f : -90f ;

        //rotate face
        yield return RotatePivot(pivot, center, RotationAngle, 0.5f);

        //add rotated gameobjects back to cubeletmap dictionary
        foreach (Transform child in pivot.transform)
        {
            GameObject cubelet = child.gameObject;
            Vector3 pos = cubelet.transform.position;
            Vector3Int cubeletPos = new(
                Mathf.RoundToInt(pos.x),
                Mathf.RoundToInt(pos.y),
                Mathf.RoundToInt(pos.z)
            );
            cubeManager.cubeletMap[cubeletPos] = cubelet;
        }

        //empty out pivot children
        while(pivot.transform.childCount > 0)
        {
            pivot.transform.GetChild(0).SetParent(cubeManager.transform);
        }

        //destroy empty pivot gameobject
        GameObject.Destroy(pivot);
    }

    //rotation animation
    IEnumerator RotatePivot(GameObject pivot, Vector3 axis, float angle, float duration)
    {
        Quaternion startRotation = pivot.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(angle, axis);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            pivot.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        pivot.transform.rotation = endRotation; 
    }

    //this function changes color of a sticker to next color in the array
    public void ChangeColor(GameObject sticker,string color)
    {
        Renderer rend = sticker.GetComponent<Renderer>();
        rend.material.color = colors[color];
        sticker.name = "Sticker_" + color;
    }

}
