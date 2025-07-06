using UnityEngine;

public class CubeStickers : MonoBehaviour
{
    [Header("Sticker Prefab")]
    public GameObject stickerPrefab;

    [Header("Sticker Materials")]
    public Material matUp;
    public Material matDown;
    public Material matLeft;
    public Material matRight;
    public Material matBack;
    public Material matFront;

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

    private void CreateSticker(GameObject cubelet, Vector3 normal, Material mat)
    {
        GameObject sticker = Instantiate(stickerPrefab, cubelet.transform);
        sticker.transform.localScale = new Vector3(0.95f, 0.95f, 0.01f);
        sticker.transform.localPosition = normal * 0.51f; // slightly outside face
        sticker.transform.localRotation = Quaternion.LookRotation(-normal); // make sticker face outward
        sticker.transform.localScale = Vector3.one * 0.9f; // optional: fit it nicely
        sticker.GetComponent<Renderer>().material = mat;
    }
}
