using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public Cubelet cube;
    public Dictionary<Vector3Int, GameObject> cubeletMap = new();
    private bool isRotating = false;

    public List<Vector3Int> topFace = GetFace(1,1);
    public List<Vector3Int> bottomFace = GetFace(1, -1);
    public List<Vector3Int> frontFace = GetFace(2, 1);
    public List<Vector3Int> backFace = GetFace(2, -1);
    public List<Vector3Int> rightFace = GetFace(0, 1);
    public List<Vector3Int> leftFace = GetFace(0, -1);

    //instanciates 9 vector3Int positions from an axis key and its value
    //axis corresponding keys: x = 0 , y = 1 , z = 2
    public static List<Vector3Int> GetFace(int axis, int value)
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


    // Rotates cube face
    public IEnumerator RotateFace(List<Vector3Int> face, bool clockwise,Vector3 center)
    {
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
            originalCubletPositions.Add(pos, cubeletMap[pos]);
            cubeletMap.Remove(pos);
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
            cubeletMap[cubeletPos] = cubelet;
        }

        //empty out pivot children
        while(pivot.transform.childCount > 0)
        {
            pivot.transform.GetChild(0).SetParent(null);
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


    void Start()
    {
        BuildCube();
    }

    void Update()
    {
        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(RotateFace(frontFace, true, frontFace[4]));
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(RotateFace(frontFace, false, frontFace[4]));
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(RotateFace(rightFace, true, rightFace[4]));
            }
        }
    }

    void BuildCube()
    {

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector3Int pos = new(x, y, z);
                    GameObject cubelet = cube.CreateCubelet(x, y, z);
                    cubeletMap[pos] = cubelet;
                }
            }
        }
    }
}
