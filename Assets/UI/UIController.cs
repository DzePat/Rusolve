using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BeginnerSolve;
using System.Linq;

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

    private ButtonController menuSlow;
    private ButtonController menuFast;
    private ButtonController solveButton;
    private ButtonController nextButton;
    private ButtonController prevButton;
    private ButtonController menuButton;
    private ButtonController zoomInButton;
    private ButtonController zoomOutButton;

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
        uiManager.CreateColorPanel();
        uiManager.CreateColorStatistics();
        uiManager.CreateSidePanel();
    }

    /// <summary>
    /// prerendering of buttons on game start
    /// </summary>
    private void CreateButtons()
    {
        //create UI buttons
        menuSlow = buttonManager.CreateButton(uiManager.uiContainer, new(0, 2, 0), new(8, 2), 1f, "Beginner", new Color32(255, 209, 97, 255));
        menuFast = buttonManager.CreateButton(uiManager.uiContainer, new(0, 0, 0), new(8, 2), 1f, "Fast(Kociemba)", new Color32(255, 209, 97, 255));
        solveButton = buttonManager.CreateButton(uiManager.uiContainer, new(0, -4, 0), new(4, 2), 1f, "solve", new Color32(255, 209, 97, 255));
        nextButton = buttonManager.CreateButton(uiManager.uiContainer, new(3, -4, 0), new(5, 2), 1f, "next", new Color32(255, 209, 97, 255));
        prevButton = buttonManager.CreateButton(uiManager.uiContainer, new(-3, -4, 0), new(5, 2), 1f, "previous", new Color32(255, 209, 97, 255));
        menuButton = buttonManager.CreateButton(uiManager.sidePanel, new(0, 4f, 0), new(4, 1), 0.5f, "Main Menu", new Color32(255, 209, 97, 255));
        zoomInButton = buttonManager.CreateButton(uiManager.sidePanel, new(-0.5f, -4f, 0), new(1, 1), 1f, "+", new Color32(255, 255, 255, 255));
        zoomOutButton = buttonManager.CreateButton(uiManager.sidePanel, new(0.5f, -4f, 0), new(1, 1), 1f, "-", new Color32(255, 255, 255, 255));

        //deactivate buttons
        HideAllButtons();


        //Subscribe buttons
        menuFast.OnClicked += HandleButtonClicked;
        menuSlow.OnClicked += HandleButtonClicked;
        solveButton.OnClicked += HandleButtonClicked;
        nextButton.OnClicked += HandleButtonClicked;
        prevButton.OnClicked += HandleButtonClicked;
        menuButton.OnClicked += HandleButtonClicked;
        zoomInButton.OnClicked += HandleButtonClicked;
        zoomOutButton.OnClicked += HandleButtonClicked;

        //Create and and asign color panel buttons
        var colors = new (Color color, Vector3 pos, string name)[]
        {
            (Color.white,  new Vector3(-1f,  2f, 0f), "white"),
            (Color.yellow, new Vector3( 1f,  2f, 0f), "yellow"),
            (Color.red,    new Vector3(-1f,  0f, 0f), "red"),
            (Color.orange, new Vector3( 1f,  0f, 0f), "orange"),
            (Color.green,  new Vector3(-1f, -2f, 0f), "green"),
            (Color.blue,   new Vector3( 1f, -2f, 0f), "blue"),
        };

        foreach (var (color, pos, name) in colors)
        {
            ButtonController bc = buttonManager.CreateColorPanelBtn(uiManager.colorPanel, color, pos, name);
            bc.OnClicked += _ => ColorPicked(name);
        }

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
        foreach(GameObject cubelet in solveController.solveManager.cubeController.cubeManager.cubeletMap.Values)
        {
            Destroy(cubelet);
        }
    }

    /// <summary>
    /// handles button clicks
    /// </summary>
    /// <param name="button"></param>
    private void HandleButtonClicked(ButtonController button)
    {
        if(button == menuFast)
        {
            MenuSolverOptionClicked("Fast");
        }
        else if(button == menuSlow)
        {
            MenuSolverOptionClicked("Slow");
        }
        else if (button == solveButton)
        {
            if (solveController.solveManager.rotationQueue.Count == 0)
            {
                SolveClicked();
            }
        }
        else if (button == nextButton)
        {
            MoveNext();
        }
        else if (button == prevButton)
        {
            MovePrevious();
        } else if (button == menuButton)
        {
            SidePanelMenuClicked();
        }
        else if (button == zoomInButton)
        {
            mainCamera.ZoomIn();
        }
        else if (button == zoomOutButton)
        {
            mainCamera.ZoomOut();
        }
    }

    /// <summary>
    /// sidepanel menu button event handler
    /// </summary>
    void SidePanelMenuClicked()
    {
        selectedSolver = "";
        HideAllButtons();
        HideAllUis();
        DestroyCube();
        uiManager.CountReset();
        showMenu();
        uiManager.sidePanel.SetActive(false);
        ClearValues();
    }

    /// <summary>
    /// hides solve, next and previous buttons
    /// </summary>
    void HideAllButtons()
    {
        solveButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// hide colorpanel and statistics panel
    /// </summary>
    void HideAllUis()
    {
        uiManager.HideStatistics();
        uiManager.colorPanel.SetActive(false);
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
    /// Hide menu buttons
    /// </summary>
    void hideMenu()
    {
        menuFast.gameObject.SetActive(false);
        menuSlow.gameObject.SetActive(false);
    }

    /// <summary>
    /// Display menu buttons
    /// </summary>
    void showMenu()
    {
        menuFast.gameObject.SetActive(true);
        menuSlow.gameObject.SetActive(true);
    }
    /// <summary>
    /// menu solver choice selected either fast or beginner
    /// </summary>
    void MenuSolverOptionClicked(string option)
    {
        hideMenu();
        uiManager.sidePanel.SetActive(true);
        uiManager.ShowStatistics();
        uiManager.ShowSidePanelRotationButtons();
        solveController.solveManager.cubeController.cubeManager.BuildCube();
        solveButton.gameObject.SetActive(true);
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
                    Debug.Log("adding: " + s);
                    solution += s;
                }
                Debug.Log($"solution: {solution}");
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
                    uiManager.HideSidePanelRotationButtons();
                    uiManager.colorPanel.SetActive(false);
                    solveButton.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(true);
                    prevButton.gameObject.SetActive(true);
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
            Debug.Log("step: " + step);
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
        uiManager.colorPanel.SetActive(true);
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
        uiManager.colorPanel.SetActive(false);
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
            uiManager.colorPanel.SetActive(false);
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
