using Assets.Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public CameraController mainCamera;
    public SolveController solveController;
    public ButtonManager buttonManager;

    private GameObject selectedSticker;
    private string previousColor;
    private string selectedSolver;
    public int solutionIndex = 0;

    public GameObject mainMenu;
    public GameObject toMainMenu;
    public GameObject hud;
    public GameObject rotationMenu;
    public TextMeshProUGUI Popup;

    //Hud objects
    public GameObject solve;
    public GameObject steps;
    public GameObject colorPanel;
    public GameObject clockwiseBtns;
    public GameObject cclockwiseBtns;

    ColorCount statistics = new ColorCount();

    void Start()
    {
    }

    public void BeginnerSolveClick()
    {
        MenuSolverOptionClicked("Slow");
    }

    public void FastSolveClick()
    {
        MenuSolverOptionClicked("Fast");
    }

    public void ToMainMenuClick()
    {
        solveController.solveManager.cubeController.isActive = false;
        selectedSolver = "";
        ResetAndHideHud();
        DestroyCube();
        statistics.Reset();
        mainMenu.SetActive(true);
        ClearValues();
    }

    public void SolveClick()
    {
        if (solveController.solveManager.rotationQueue.Count == 0 && solveController.solveManager.cubeController.isRotating == false)
        {
            SolveClicked();
        }
    }

    public void DirectionClick()
    {
        if (clockwiseBtns.activeSelf == false)
        {
            clockwiseBtns.SetActive(true);
            cclockwiseBtns.SetActive(false);
        }
        else
        {
            clockwiseBtns.SetActive(false);
            cclockwiseBtns.SetActive(true);
        }
    }

    /// <summary>
    /// rotation handler for cube sides from sidepanel
    /// </summary>
    /// <param name="rotation"></param>
    public void RotateEvent(string rotation)
    {
        List<Vector3Int> face = solveController.solveManager.moveMap[rotation[0]];
        bool rotateclockwise = rotation.Length > 1 ? false : true;
        solveController.solveManager.EnqueueRotation(face, rotateclockwise);
    }

    /// <summary>
    /// clears cube list
    /// </summary>
    void DestroyCube()
    {
        foreach (GameObject cubelet in solveController.solveManager.cubeController.cubeManager.cubeletMap.Values)
        {
            if (cubelet != null)
                Destroy(cubelet);
        }
    }

    /// <summary>
    /// hides solve, next and previous buttons
    /// </summary>
    private void ResetAndHideHud()
    {
        solve.SetActive(true);
        steps.SetActive(false);
        rotationMenu.SetActive(true);
        hud.SetActive(false);
    }

    /// <summary>
    /// clears user selecte values like selected sticker
    /// </summary>
    private void ClearValues()
    {
        selectedSticker = null;
        previousColor = null;
        solutionIndex = 0;
        solveController.cubeSolution = null;
    }


    /// <summary>
    /// menu solver choice selected either fast or beginner
    /// </summary>
    private void MenuSolverOptionClicked(string option)
    {
        mainMenu.SetActive(false);
        toMainMenu.SetActive(true);
        hud.SetActive(true);
        solveController.solveManager.cubeController.isActive = true;
        solveController.solveManager.cubeController.cubeManager.BuildCube();
        selectedSolver = option;
    }

    /// <summary>
    /// Solve button click event
    /// </summary>
    private void SolveClicked()
    {    
        if (statistics.AllColorsEqual())
        {
            try
            { 
                solveController.cubeSolution = selectedSolver == "Fast" ? solveController.GetKociembaSolution() : solveController.GetBeginnerSolution();
                string solution = "";
                foreach(string s in solveController.cubeSolution)
                {
                    solution += s;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Excpetion while solving: {e}");
            }
            if (solveController.cubeSolution != null)
            {
                if (solveController.cubeSolution[0] != "None")
                {
                    solveController.solveManager.cubeController.DisableStickerClick();
                    colorPanel.SetActive(false);
                    rotationMenu.SetActive(false);
                    solve.SetActive(false);
                    steps.SetActive(true);
                }
                else
                {
                    ShowPopup("Cube is in a <color=green>solved</color> state");
                }
            }
            else
            {
                ShowPopup("invalid cube state , no solution found");
            }
        }
        else
        {
            ShowPopup(ColorError());
        }
    }


    /// <summary>
    /// Executes the rotation for next step
    /// </summary>
    public void NextClick()
    {
        if (solutionIndex != solveController.cubeSolution.Length)
        {
            string step = solveController.cubeSolution[solutionIndex];
            List<Vector3Int> face = solveController.solveManager.moveMap[step[0]];
            if (step.Length == 1)
            {
                solveController.solveManager.EnqueueRotation(face, true);
            }
            else
            {
                if (step[1] == '2')
                {
                    solveController.solveManager.EnqueueRotation(face, true);
                    solveController.solveManager.EnqueueRotation(face, true);
                }
                else
                {
                    solveController.solveManager.EnqueueRotation(face, false);
                }
            }
            solutionIndex++;
        }
    }

    /// <summary>
    /// Executes the rotation for previous step
    /// </summary>
    public void PreviousClick()
    {
        if (solutionIndex != 0)
        {
            solutionIndex--;
            string step = solveController.cubeSolution[solutionIndex];
            List<Vector3Int> face = solveController.solveManager.moveMap[step[0]];
            if (step.Length == 1)
            {
                solveController.solveManager.EnqueueRotation(face, false);
            }
            else
            {
                if (step[1] == '2')
                {
                    solveController.solveManager.EnqueueRotation(face, false);
                    solveController.solveManager.EnqueueRotation(face, false);
                }
                else
                {
                    solveController.solveManager.EnqueueRotation(face, true);
                }
            }
        }
    }

    /// <summary>
    /// Sets global variables for sticker colors and highlights current sticker before enabling a color panel
    /// </summary>
    /// <param name="clicked"></param>
    public void HandleStickerClicked(GameObject clicked)
    {
        if (selectedSticker != null)
        {
            solveController.solveManager.cubeController.ChangeColor(selectedSticker, previousColor);
            statistics.Add(previousColor);
        }
        selectedSticker = clicked;
        string[] StickerName = selectedSticker.name.Split('_');
        previousColor = StickerName[1];
        statistics.Sub(previousColor);
        solveController.solveManager.cubeController.ChangeColor(selectedSticker, "temp");
        colorPanel.SetActive(true);
    }

    /// <summary>
    /// changes color of a selected sticker based on users choice on the color panel
    /// </summary>
    /// <param name="color"></param>
    public void ColorPicked(string color)
    {
        solveController.solveManager.cubeController.ChangeColor(selectedSticker, color);
        selectedSticker = null;
        previousColor = null;
        statistics.Add(color);
        colorPanel.SetActive(false);
    }

    /// <summary>
    /// checks if a sticker has been selected before and if it is returns it to original state and clears user selection values
    /// </summary>
    public void resetStickerSelection()
    {
        if (selectedSticker != null && previousColor != null)
        {
            solveController.solveManager.cubeController.ChangeColor(selectedSticker, previousColor);
            statistics.Add(previousColor);
            colorPanel.SetActive(false);
            ClearValues();
        }
    }

    private void Update()
    {
        // Pointer handles Mouse and the primary Touch automatically
        if (Pointer.current != null && Pointer.current.press.wasReleasedThisFrame)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.name.StartsWith("Sticker_"))
                    HandleStickerClicked(hit.collider.gameObject);
                else
                    resetStickerSelection();
            }
            else
            {
                resetStickerSelection();
            }
        }
    }

    public void ShowPopup(string message)
    {
        Popup.gameObject.SetActive(true);
        Popup.text = message;
        StartCoroutine(HideAfterSeconds(2f));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Popup.gameObject.SetActive(false);
    }

    private string ColorError()
    {
        string result = "";

        ColorStr(ref result, statistics.white, "white");
        ColorStr(ref result, statistics.green, "green");
        ColorStr(ref result, statistics.blue, "blue");
        ColorStr(ref result, statistics.yellow, "yellow");
        ColorStr(ref result, statistics.red, "red");
        ColorStr(ref result, statistics.orange, "orange");

        return result;
    }

    private void ColorStr(ref string result, int value, string color)
    {
        if (value < 9)
        {
            result += $"Missing {9 - value} <color={color}>{color}</color>.\n";
        }
    }

}
