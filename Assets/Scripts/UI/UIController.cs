using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BeginnerSolve;
using System.Linq;
using Assets.BeginnerSolver;
using UnityEngine.SceneManagement;
using PlasticGui;

public class UIController : MonoBehaviour
{
    public CameraController mainCamera;
    public UIManager uiManager;
    public SolveController solveController;
    public ButtonManager buttonManager;

    private GameObject selectedSticker;
    private string previousColor;
    private string selectedSolver;
    public int solutionIndex = 0;

    public GameObject mainMenu;
    public GameObject sideMenu;
    public GameObject hud;

    //Hud objects
    public GameObject solve;
    public GameObject steps;
    public GameObject colorPanel;

    void Start()
    {
        CreateUIContainers();
        CreateButtons();
    }
    /// <summary>
    /// prerendering of UI containers on game start
    /// </summary>
    private void CreateUIContainers()
    {
        // Create UI containers
        uiManager.CreateUiContainer();
        uiManager.CreateColorStatistics();
        uiManager.CreateSidePanel();
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
        HideAllUis();
        DestroyCube();
        uiManager.CountReset();
        sideMenu.SetActive(false);
        mainMenu.SetActive(true);
        uiManager.sidePanel.SetActive(false);
        ClearValues();
    }

    public void SolveClick()
    {
        if (solveController.solveManager.rotationQueue.Count == 0 && solveController.solveManager.cubeController.isRotating == false)
        {
            SolveClicked();
        }
    }

    public void NextClick()
    {
        MoveNext();
    }

    public void PreviousClick()
    {
        MovePrevious();
    }
    /// <summary>
    /// prerendering of buttons on game start
    /// </summary>
    private void CreateButtons()
    {
        //Create rotation buttons and asign events
        var rotations = new (string rotation, Vector3 pos)[]
        {
            ("R",new (-0.5f,3f, 0)),
            ("R'",new (0.5f,3f, 0)),
            ("L",new (-0.5f,2f, 0)),
            ("L'",new (0.5f,2f, 0)),
            ("U",new (-0.5f,1f, 0)),
            ("U'",new (0.5f,1f, 0)),
            ("D",new (-0.5f,0f, 0)),
            ("D'",new (0.5f,0f, 0)),
            ("B",new (-0.5f,-1f, 0)),
            ("B'",new (0.5f,-1f, 0)),
            ("F",new (-0.5f,-2f, 0)),
            ("F'",new (0.5f,-2f, 0)),
        };

        GameObject rotContainer = uiManager.sidePanel.transform.Find("rotContainer").gameObject;

        foreach (var (rot,pos) in rotations)
        {
            ButtonController bc = buttonManager.CreateButton(rotContainer, pos, new(1, 1), 0.5f, rot, new Color32(255, 255, 255, 255));
            bc.OnClicked += _ => RotateEvent(rot);
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
    void ResetAndHideHud()
    {
        solve.SetActive(true);
        steps.SetActive(false);
        hud.SetActive(false);
    }

    /// <summary>
    /// hide colorpanel and statistics panel
    /// </summary>
    void HideAllUis()
    {
        uiManager.HideStatistics();
        colorPanel.SetActive(false);
    }

    /// <summary>
    /// clears user selecte values like selected sticker
    /// </summary>
    void ClearValues()
    {
        selectedSticker = null;
        previousColor = null;
        solutionIndex = 0;
        solveController.cubeSolution = null;
    }


    /// <summary>
    /// menu solver choice selected either fast or beginner
    /// </summary>
    void MenuSolverOptionClicked(string option)
    {
        mainMenu.SetActive(false);
        sideMenu.SetActive(true);
        hud.SetActive(true);
        solve.SetActive(true);
        solveController.solveManager.cubeController.isActive = true;
        uiManager.sidePanel.SetActive(true);
        uiManager.ShowStatistics();
        uiManager.ShowSidePanelRotationButtons();
        solveController.solveManager.cubeController.cubeManager.BuildCube();
        selectedSolver = option;
    }

    bool EqualNumberOfColors()
    {
        bool equalNumberOfColors = true;
        foreach (TMP_Text colorCount in uiManager.colorsStats.Values)
        {
            if (colorCount.text != "9")
            {
                equalNumberOfColors = false;
            }
        }
        return equalNumberOfColors;
    }

    /// <summary>
    /// Solve button click event
    /// </summary>
    void SolveClicked()
    {
        if (EqualNumberOfColors())
        {
            try
            { 
                solveController.cubeSolution = selectedSolver == "Fast" ? solveController.GetKociembaSolution() : solveController.GetBeginnerSolution();
                string solution = "";
                foreach(string s in solveController.cubeSolution)
                {
                    solution += s;
                }
                Debug.Log($"solution: {solution}");
            }
            catch (Exception e)
            {
                Debug.Log($"Excpetion while solving: {e}");
            }
            Debug.Log("test: " + solveController.cubeSolution);
            if (solveController.cubeSolution != null)
            {
                if (solveController.cubeSolution[0] != "None")
                {
                    solveController.solveManager.cubeController.DisableStickerClick();
                    uiManager.HideSidePanelRotationButtons();
                    colorPanel.SetActive(false);
                    solve.SetActive(false);
                    steps.SetActive(true);
                    uiManager.HideStatistics();
                }
                else
                {
                    uiManager.ShowPopup("Cube is in a solved state", new Vector3(0, 3.5f, 0));
                }
            }
            else
            {
                uiManager.ShowPopup("invalid cube state , no solution found", new Vector3(0, 3.5f, 0));
            }
        }
        else
        {
            uiManager.ShowPopup("the number of stickers for each color must be exactly 9.", new Vector3(0, 3.5f, 0));
        }
    }


    /// <summary>
    /// Executes the rotation for next step
    /// </summary>
    public void MoveNext()
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
    public void MovePrevious()
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
            uiManager.CountAdd(previousColor);
        }
        selectedSticker = clicked;
        string[] StickerName = selectedSticker.name.Split('_');
        previousColor = StickerName[1];
        uiManager.CountSub(previousColor);
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
        uiManager.CountAdd(color);
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
            uiManager.CountAdd(previousColor);
            colorPanel.SetActive(false);
            ClearValues();
        }
    }

    private void Update()
    {
        Vector3? pointerPos = null;

        if (Input.GetMouseButtonUp(0)) // release = click
            pointerPos = Input.mousePosition;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended && touch.tapCount == 1)
                pointerPos = touch.position;
        }

        if (pointerPos.HasValue)
        {
            Ray ray = Camera.main.ScreenPointToRay(pointerPos.Value);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clicked = hit.collider.gameObject;
                if (clicked.name.StartsWith("Sticker_"))
                    HandleStickerClicked(clicked);
                else
                {
                    resetStickerSelection();
                }
            }
            else
            {
                resetStickerSelection();
            }

        }
    }
}
