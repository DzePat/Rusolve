using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float distance = 12f;
    public float zoomSpeed = 0.01f;
    public float minDistance = 10f;
    public float maxDistance = 20f;

    public float xSpeed = 120f;
    public float ySpeed = 120f;

    float x;
    float y;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        HandleRotation();
        HandleZoom();

        y = Mathf.Clamp(y, -80f, 80f);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position;

        transform.SetPositionAndRotation(position, rotation);
    }

    void HandleRotation()
    {
        var pointer = Pointer.current;
        if (pointer == null)
            return;

        if (pointer.press.isPressed)
        {
            Vector2 delta = pointer.delta.ReadValue();

            x += delta.x * xSpeed * Time.deltaTime;
            y -= delta.y * ySpeed * Time.deltaTime;
        }
    }


    void HandleZoom()
    {
        float zoom = 0f;

        // 🖱 Mouse scroll (PC)
        if (Mouse.current != null)
            zoom += Mouse.current.scroll.ReadValue().y;

        // 📱 Pinch zoom (Mobile)
        var touches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;

        if (touches.Count >= 2)
        {
            var t0 = touches[0];
            var t1 = touches[1];

            Vector2 t0Prev = t0.screenPosition - t0.delta;
            Vector2 t1Prev = t1.screenPosition - t1.delta;

            float prevDist = Vector2.Distance(t0Prev, t1Prev);
            float currDist = Vector2.Distance(t0.screenPosition, t1.screenPosition);

            zoom += (currDist - prevDist);
        }

        distance -= zoom * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void ApplyZoom(float delta)
    {
        distance -= delta * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    public void ZoomIn()
    {
        ApplyZoom(-1f);
    }

    public void ZoomOut()
    {
        ApplyZoom(1f);
    }
}