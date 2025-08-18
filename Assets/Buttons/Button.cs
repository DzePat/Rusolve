using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public GameObject buttonprefab;

    //Add two buttons next and previous
    public void CreateSolveNavigationButtons()
    {
        Vector3Int nextButtonPos = new Vector3Int(3, -2, -5);
        GameObject nextButtonObj = Instantiate(buttonprefab);
        nextButtonObj.transform.position = nextButtonPos;
        nextButtonObj.transform.localScale *= 1.01f;
        TMP_Text labelOne = nextButtonObj.GetComponentInChildren<TMP_Text>();
        nextButtonObj.name = "Next";
        labelOne.text = "Next";
        labelOne.fontSize = 1;

        Vector3Int previousButtonPos = new Vector3Int(-3, -2, -5);
        GameObject previousButtonObj = Instantiate(buttonprefab);
        previousButtonObj.transform.position = previousButtonPos;
        TMP_Text labelTwo = previousButtonObj.GetComponentInChildren<TMP_Text>();
        previousButtonObj.name = "Previous";
        labelTwo.text = "Previous";
        labelTwo.fontSize = 1;

    }

    public void CreateStartSolveButton()
    {
        Vector3Int ButtonPos = new Vector3Int(0, -2, -5);
        GameObject ButtonObj = Instantiate(buttonprefab, transform);
        ButtonObj.transform.position = ButtonPos;
        TMP_Text labelTwo = ButtonObj.GetComponentInChildren<TMP_Text>();
        ButtonObj.name = "Solve";
        labelTwo.text = "Solve";
        labelTwo.fontSize = 1;

        ButtonObj.AddComponent<BoxCollider>();
        var click = ButtonObj.AddComponent<ButtonController>();
        click.parentButton = this;
    }

    public void ClickSolve(GameObject button)
    {
        button.SetActive(false);
        CreateSolveNavigationButtons();
    }


    void Start()
    {
        CreateStartSolveButton();
    }

    void Update()
    {


    }
}
