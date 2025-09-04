using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{

    public CubeManager cubeManager;
    private readonly Queue<(List<Vector3Int> face, bool clockwise)> rotationQueue = new();
    public bool isRotating = false;
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

    public List<Vector3Int> topFace = GetFacePositions(1, 1);
    public List<Vector3Int> bottomFace = GetFacePositions(1, -1);
    public List<Vector3Int> backFace = GetFacePositions(2, 1);
    public List<Vector3Int> frontFace = GetFacePositions(2, -1);
    public List<Vector3Int> rightFace = GetFacePositions(0, 1);
    public List<Vector3Int> leftFace = GetFacePositions(0, -1);

    /// <summary>
    /// Adds a rotation to the queue
    /// </summary>
    /// <param name="face"></param>
    /// <param name="clockwise"></param>
    public void EnqueueRotation(List<Vector3Int> face, bool clockwise)
    {
        rotationQueue.Enqueue((face, clockwise));

        if (!isRotating)
            StartCoroutine(ProcessQueue());
    }

    /// <summary>
    /// Dequeues enqueued rotations
    /// </summary>
    private IEnumerator ProcessQueue()
    {
        isRotating = true;

        while (rotationQueue.Count > 0)
        {
            var (face, clockwise) = rotationQueue.Dequeue();
            yield return StartCoroutine(RotateFace(face, clockwise));
        }

        isRotating = false;
    }

    /// <summary>
    /// Takes and axis x = 0 , y = 1 , z = 2 and a value 1 or -1 and returns a list of positions of face cubelets
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="value"></param>
    /// <returns> returns a list with 9 face cubelet positions</returns>
    public static List<Vector3Int> GetFacePositions(int axis, int value)
    {
        List<Vector3Int> face = new();
        int a = (axis + 1) % 3;
        int b = (axis + 2) % 3;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int[] pos = new int[3];
                pos[axis] = value;
                pos[a] = i;
                pos[b] = j;
                face.Add(new Vector3Int(pos[0], pos[1], pos[2]));
            }
        } 
        return face;
    }

    /// <summary>
    /// Takes a face as argument and returns a list of sticker positions and their corresponding colors
    /// </summary>
    /// <param name="face"></param>
    /// <returns>list of 9 sticker positions and their colors</returns>
    public Dictionary<Vector3Int, string> GetFaceColors(List<Vector3Int> face)
    {
        Vector3Int center = face[4];
        Dictionary<Vector3Int, string> FaceColors = new();

        //get vector identifier x ,y or z
        int position = Mathf.Abs(center.x) == 1 ? 0 : Mathf.Abs(center.y) == 1 ? 1 : 2;

        //vector value to compare against cubelet children
        int value = position == 0 ? center.x : position == 1? center.y : center.z;

        //change compare value to 2 because the offset for sticker values are 0.51 and after rounding up its 2.
        float compareValue = value >= 1 ? 2 : -2;

        int childcount = 0;
        //iterate trough 9 face cubelets
        foreach (Vector3Int pos in face)
        {
            GameObject cubelet = cubeManager.cubeletMap[pos];
            childcount++;

            //iterate trough cubelet children
            foreach (Transform child in cubeManager.cubeletMap[pos].transform)
            {
                Vector3 stickerPos = child.position;

                //round compare axis to get more accurate compare value of 2 or -2
                int comparePos = Mathf.RoundToInt(
                    position == 0 ? stickerPos.x :
                    position == 1 ? stickerPos.y :
                    stickerPos.z
                );

                //find the sticker facing the right direction
                if (comparePos == compareValue)
                {
                    string stickerColor = child.transform.name;
                    FaceColors.Add(pos, stickerColor);
                }
            }
        }

        return FaceColors;
    }

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

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EnqueueRotation(frontFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            EnqueueRotation(leftFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            EnqueueRotation(rightFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            EnqueueRotation(topFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            EnqueueRotation(bottomFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            EnqueueRotation(backFace, true);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {

            Dictionary<Vector3Int,string> FaceColors = GetFaceColors(topFace);
            foreach (var faceColor in FaceColors)
            {
                Debug.Log($"Position: {faceColor.Key} Color: {faceColor.Value}");
            }
        }
        
    }
}
