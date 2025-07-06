using UnityEngine;

public class CubeBuilder : MonoBehaviour
{
    public GameObject cubeletPrefab; // A plain cube
    public CubeStickers stickerManager;
    public Material Material;
    
    void Start()
    {
        BuildCube();
    }

    void BuildCube()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);
                    GameObject cubelet = Instantiate(cubeletPrefab, transform);
                    cubelet.transform.localPosition = pos;
                    cubelet.transform.localRotation = Quaternion.identity;
                    cubelet.transform.localScale = Vector3.one;
                    cubelet.name = $"Cubelet_{x}_{y}_{z}";
                    cubelet.GetComponent<Renderer>().material = Material;

                    // Add stickers based on position
                    stickerManager.AddStickers(cubelet, pos);
                }
            }
        }
    }
}
