using UnityEngine;

public class UIController : MonoBehaviour
{
    public ButtonController buttonController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonController.buttonManager.uiManager.CreateUiContainer();
        buttonController.buttonManager.CreateColorPanel();
        buttonController.buttonManager.CreateMenuKociembaButton();
        buttonController.buttonManager.uiManager.CreateColorStatistics();
    }

}
