using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public ButtonManager buttonManager;
    public Transform cameraTransform;
    public Vector3 offset = new Vector3(0, -3, 14);

    public void OnButtonClicked(GameObject buttonObj)
    {
        Button button = buttonObj.GetComponent<Button>();
        if (button.name == "Solve")
        {
            Destroy(buttonObj);
            buttonManager.ClickSolve();
        }
    }


    //FIXME rotating over the y axis snaps the canvas to the middle and rotates by 180 degrees which it shouldnt do
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


        transform.LookAt(cameraTransform.localPosition,Vector3.up);
        
        transform.Rotate(0, 180f, 0f);

    }

}
