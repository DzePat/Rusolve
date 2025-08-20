using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ButtonController : MonoBehaviour
{
    public ButtonManager buttonManager;
    public CubeController cubeController;

    public void OnButtonClicked(GameObject buttonObj)
    {
        Button button = buttonObj.GetComponent<Button>();
        if (button.name == "Solve")
        {
            Destroy(buttonObj);
            cubeController.DisableStickerClick();
            buttonManager.CreateSolveNavigationButtons();
        }
    }

}
