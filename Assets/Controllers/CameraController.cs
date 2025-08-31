using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CameraController : MonoBehaviour
{
    void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    public Transform target;
    public float distance = 12.0f;
    public float zoomSpeed = 3f;
    public float minDistance = 10f;
    public float maxDistance = 20f;

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

        //dev


        if (Input.touchSupported && Input.touchCount >= 2)
        {
            // Mobile with at least 2 touches
            Vector2 delta1 = Input.GetTouch(0).deltaPosition;
            Vector2 delta2 = Input.GetTouch(1).deltaPosition;

            Vector2 averageDelta = (delta1 + delta2) / 2;

            x += averageDelta.x * xSpeed * 0.3f * Time.deltaTime;
            y -= averageDelta.y * ySpeed * 0.3f * Time.deltaTime;
        }
        else if (Input.mousePresent && Input.GetMouseButton(1))  // Right mouse button pressed
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            x += mouseDelta.x * xSpeed * Time.deltaTime;
            y -= mouseDelta.y * ySpeed * Time.deltaTime;
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
