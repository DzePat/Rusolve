using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour
{
    public Button parentButton;
    public Transform cameraTransform;
    public Vector3 offset = new Vector3(0, -2, 5);

    void OnMouseDown()
    {
        string buttonName = parentButton.transform.GetChild(0).name;
        if (buttonName == "Solve")
        {
            parentButton.ClickSolve(parentButton.gameObject);
        }
    }

    void LateUpdate()
    {
        if(cameraTransform == null)
            cameraTransform = Camera.main?.transform;

        if (cameraTransform == null)
            return;

        Vector3 desiredPosition = cameraTransform.position
                            + cameraTransform.right * offset.x
                            + cameraTransform.up * offset.y
                            + cameraTransform.forward * offset.z;

        transform.position = desiredPosition;


        transform.LookAt(cameraTransform.position);
        
        transform.Rotate(0, 180f, 0);

        Debug.Log(transform.position);
    }

}
