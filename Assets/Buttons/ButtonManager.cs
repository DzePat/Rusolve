using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject canvasPrefab;
    public GameObject buttonPrefab;

    public ButtonController btnController;
    private GameObject uiContainer;

    //Add two buttons next and previous
    public void CreateSolveNavigationButtons()
    {
        Vector3Int nextButtonPos = new Vector3Int(3, -3, -2);
        GameObject nextButtonObj = Instantiate(buttonPrefab,uiContainer.transform);
        nextButtonObj.transform.localPosition = nextButtonPos;
        TMP_Text labelOne = nextButtonObj.GetComponentInChildren<TMP_Text>();
        nextButtonObj.name = "Next";
        labelOne.text = "Next";
        labelOne.fontSize = 1;

        Vector3Int previousButtonPos = new Vector3Int(-3, -3, -2);
        GameObject previousButtonObj = Instantiate(buttonPrefab, uiContainer.transform);
        previousButtonObj.transform.localPosition = previousButtonPos;
        TMP_Text labelTwo = previousButtonObj.GetComponentInChildren<TMP_Text>();
        previousButtonObj.name = "Previous";
        labelTwo.text = "Previous";
        labelTwo.fontSize = 1;

    }

    public void CreateStartSolveButton()
    {
        Vector3Int buttonPos = new Vector3Int(0, -3, -2);
        GameObject buttonObj = Instantiate(buttonPrefab, uiContainer.transform);
        buttonObj.transform.localPosition = buttonPos;
        TMP_Text labelTwo = buttonObj.GetComponentInChildren<TMP_Text>();
        labelTwo.text = "Solve";
        labelTwo.fontSize = 1;

        //asign event
        Button solveButton = buttonObj.GetComponent<Button>();
        solveButton.name = "Solve";
        solveButton.onClick.AddListener(() => btnController.OnButtonClicked(buttonObj));
    }

    public void ClickSolve()
    {
        CreateSolveNavigationButtons();
    }


    void Start()
    {
        uiContainer = Instantiate(canvasPrefab,transform);
        var click = uiContainer.AddComponent<ButtonController>();
        click.buttonManager = this;
        CreateStartSolveButton();
    }

    void Update()
    {


    }
}
