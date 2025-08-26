using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public CubeManager cubeManager;
    private bool isRotating = false;

    public List<Vector3Int> topFace = GetFacePositions(1, 1);
    public List<Vector3Int> bottomFace = GetFacePositions(1, -1);
    public List<Vector3Int> backFace = GetFacePositions(2, 1);
    public List<Vector3Int> frontFace = GetFacePositions(2, -1);
    public List<Vector3Int> rightFace = GetFacePositions(0, 1);
    public List<Vector3Int> leftFace = GetFacePositions(0, -1);

    //instanciates 9 vector3Int positions from an axis key and its value
    //axis corresponding keys: x = 0 , y = 1 , z = 2
    public static List<Vector3Int> GetFacePositions(int axis, int value)
    {
        List<Vector3Int> face = new List<Vector3Int>();
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

    //Get color of each face cubelet sticker with its position
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

    //Get nested array of a layer with real position and name position to easier track movement. Utility function.
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

    //disable cubelet sticker click events
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

    // Rotates cube face
    public IEnumerator RotateFace(List<Vector3Int> face, bool clockwise)
    {
        Vector3 center = face[4];
        isRotating = true;

        //temporary parent Gameobject for the 9 cubelets thats gonna rotate
        GameObject pivot = new GameObject("Pivot");
        
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
            Vector3Int cubeletPos = new Vector3Int(
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

        isRotating = false;
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
    public void ChangeColor(GameObject sticker)
    {
        Color[] colors = new Color[] { Color.white, Color.yellow, Color.orange, Color.red, Color.green, Color.blue };
        String[] colorNames = new string[] { "white", "yellow", "orange", "red", "green", "blue" };
        Renderer rend = sticker.GetComponent<Renderer>();
        Color currentMat = rend.sharedMaterial.color;
        int index = Array.IndexOf(colors, currentMat);
        index++;
        if (index == colors.Length) index = 0;
        rend.material.color = colors[index];
        sticker.name = "Sticker_" + colorNames[index];
    }



    void Start()
    {
        cubeManager.BuildCube();
    }

    void Update()
    {
        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(RotateFace(frontFace, true));
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(RotateFace(frontFace, false));
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(RotateFace(rightFace, true));
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
        // https://github.com/Megalomatt/Kociemba
        //handle mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clicked = hit.collider.gameObject;



                if (clicked.name.StartsWith("Sticker_"))
                {
                    ChangeColor(clicked); 
                }
            }
        }
    }
}
