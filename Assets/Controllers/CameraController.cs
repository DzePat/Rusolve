using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float zoomSpeed = 3f;
    public float minDistance = 3f;
    public float maxDistance = 100f;

    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    float x = 0.0f;
    float y = 0.0f;

    private Mouse mouse;

    void Start()
    {
        x = transform.eulerAngles.y;
        y = transform.eulerAngles.x;
        mouse = Mouse.current;
    }

    void LateUpdate()
    {
        if (target == null || mouse == null)
            return;

        if (mouse.rightButton.isPressed)
        {
            x += mouse.delta.x.ReadValue() * xSpeed * Time.deltaTime;
            y -= mouse.delta.y.ReadValue() * ySpeed * Time.deltaTime;
        }

        float scrollValue = mouse.scroll.ReadValue().y;
        distance -= scrollValue * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}
