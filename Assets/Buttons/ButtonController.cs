using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public ButtonManager buttonManager;
    public SolveController solveController;
    private GameObject selectedSticker;
    private string previousColor;

    private string[] solution;
    private int solutionIndex = 0;

    public void OnButtonClicked(GameObject buttonObj)
    {
        if (!solveController.cubeController.isRotating)
        {
            Button button = buttonObj.GetComponent<Button>();
            if (button.name == "Solve")
            {
                Destroy(buttonObj);
                solveController.cubeController.DisableStickerClick();
                buttonManager.CreateSolveNavigationButtons();
                solution = solveController.KociembaSolveAlgorithm();
                solveController.populateMoveMap();
            }
            if (button.name == "Solve_With_Kociemba")
            {
                Destroy(buttonObj);
                solveController.cubeController.cubeManager.BuildCube();
                buttonManager.CreateStartSolveButton();
            }
            if (button.name == "Next")
            {
                MoveNext();
            }
            if (button.name == "Previous")
            {
                MovePrevious();
            }
        }
    }

    public void MoveNext()
    {
        if (solutionIndex != solution.Length)
        {
            string step = solution[solutionIndex];
            List<Vector3Int> face = solveController.moveMap[step[0]];
            if (step.Length == 1)
            {
                solveController.cubeController.EnqueueRotation(face, true);
            }
            else
            {
                if (step[1] == '2')
                {
                    solveController.cubeController.EnqueueRotation(face, true);
                    solveController.cubeController.EnqueueRotation(face, true);
                }
                else
                {
                    solveController.cubeController.EnqueueRotation(face, false);
                }
            }
            solutionIndex++;
        }
    }

    public void MovePrevious()
    {
       if(solutionIndex != 0)
        {
            solutionIndex--;
            string step = solution[solutionIndex];
            List<Vector3Int> face = solveController.moveMap[step[0]];
            if (step.Length == 1)
            {
                solveController.cubeController.EnqueueRotation(face, false);
            }
            else
            {
                if (step[1] == '2')
                {
                    solveController.cubeController.EnqueueRotation(face, false);
                    solveController.cubeController.EnqueueRotation(face, false);
                }
                else
                {
                    solveController.cubeController.EnqueueRotation(face, true);
                }
            }
        }
    }
    
    /// <summary>
    /// changes color of a selected sticker based on users choice on the color panel
    /// </summary>
    /// <param name="color"></param>
    public void ColorPicked(string color)
    {
        solveController.cubeController.ChangeColor(selectedSticker,color);
        selectedSticker = null;
        previousColor = null;
        buttonManager.colorPicker.SetActive(false);
    }

    /// <summary>
    /// Sets global variables for sticker colors and highlights current sticker before enabling a color panel
    /// </summary>
    /// <param name="clicked"></param>
    public void HandleStickerClicked(GameObject clicked)
    {
        if (selectedSticker != null)
        {
            solveController.cubeController.ChangeColor(selectedSticker, previousColor);
        }
        selectedSticker = clicked;
        string[] StickerName = selectedSticker.name.Split('_');
        previousColor = StickerName[1];
        solveController.cubeController.ChangeColor(selectedSticker, "temp");
        buttonManager.colorPicker.SetActive(true);
    }

    public void Update()
    {
        if (Input.touchCount == 0 && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clicked = hit.collider.gameObject;

                if (clicked.name.StartsWith("Sticker_"))
                {
                    HandleStickerClicked(clicked);
                }
            }
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    GameObject clicked = hit.collider.gameObject;

                    if (clicked.name.StartsWith("Sticker_"))
                    {
                        HandleStickerClicked(clicked);
                    }
                }
            }
        }
    }

}
