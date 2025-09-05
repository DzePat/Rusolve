using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public UIManager uiManager;
    public SolveController solveController;
    public ButtonManager buttonManager;

    private GameObject selectedSticker;
    private string previousColor;
    public int solutionIndex = 0;

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

    private void CreateUIContainers()
    {
        // Create UI containers
        uiManager.CreateUiContainer();
        uiManager.CreateColorPanel();
        uiManager.CreateColorStatistics();
        uiManager.CreateSidePanel();
    }


    private void CreateButtons()
    {
        //create UI buttons
        menuFast = buttonManager.CreateButton(uiManager.uiContainer,new(0, 0, -2), new(8, 2), "Fast(Kociemba)");
        solveButton = buttonManager.CreateButton(uiManager.uiContainer, new(0, -3, -2), new(4, 2), "solve");
        nextButton = buttonManager.CreateButton(uiManager.uiContainer, new(3, -3, -2), new(5, 2), "next");
        prevButton = buttonManager.CreateButton(uiManager.uiContainer, new(-3, -3, -2), new(5, 2), "previous");
        menuButton = buttonManager.CreateButton(uiManager.uiContainer, new(3, -3, -2), new(5, 2), "Main Menu");
        zoomInButton = buttonManager.CreateButton(uiManager.uiContainer, new(-3, -3, -2), new(5, 2), "+");
        zoomOutButton = buttonManager.CreateButton(uiManager.uiContainer, new(-3, -3, -2), new(5, 2), "-");

        //deactivate buttons
        solveButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        zoomInButton.gameObject.SetActive(false);
        zoomOutButton.gameObject.SetActive(false);

        //Subscribe buttons
        menuFast.OnClicked += HandleButtonClicked;
        solveButton.OnClicked += HandleButtonClicked;
        nextButton.OnClicked += HandleButtonClicked;
        prevButton.OnClicked += HandleButtonClicked;
        menuButton.OnClicked += HandleButtonClicked;
        zoomInButton.OnClicked -= HandleButtonClicked;
        zoomOutButton.OnClicked -= HandleButtonClicked;

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


    }

    /// <summary>
    /// handles button clicks
    /// </summary>
    /// <param name="button"></param>
    private void HandleButtonClicked(ButtonController button)
    {
        if(button == menuFast)
        {
            MenuFastClicked();

        }
        else if (button == solveButton)
        {
            SolveClicked();
        }
        else if (button == nextButton)
        {
            MoveNext();
        }
        else if (button == prevButton)
        {
            MovePrevious();
        }else if (button == menuButton)
        {

        }
    }

    /// <summary>
    /// menuFast button event
    /// </summary>
    void MenuFastClicked()
    {
        menuFast.gameObject.SetActive(false);
        uiManager.ShowStatistics();
        solveController.solveManager.cubeController.cubeManager.BuildCube();
        solveButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Solve button click event
    /// </summary>
    void SolveClicked()
    {
        bool equalNumberOfColors = true;
        foreach (TMP_Text colorCount in uiManager.colorsStats.Values)
        {
            if (colorCount.text != "9")
            {
                equalNumberOfColors = false;
            }
        }
        if (equalNumberOfColors)
        {
            try
            {
                solveController.cubeSolution = solveController.GetKociembaSolution();
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
                    solveController.solveManager.SetMoveMap();
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

    private void Update()
    {
        Vector3? pointerPos = null;

        // mouse
        if (Input.GetMouseButtonDown(0))
            pointerPos = Input.mousePosition;

        // touch
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            pointerPos = Input.GetTouch(0).position;

        if (pointerPos.HasValue)
        {
            Ray ray = Camera.main.ScreenPointToRay(pointerPos.Value);
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
