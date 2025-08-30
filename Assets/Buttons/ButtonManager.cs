using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

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

        Button nextButton = nextButtonObj.GetComponent<Button>();
        nextButton.name = "Next";
        nextButton.onClick.AddListener(() => btnController.OnButtonClicked(nextButtonObj));

        Vector3Int previousButtonPos = new Vector3Int(-3, -3, -2);
        GameObject previousButtonObj = Instantiate(buttonPrefab, uiContainer.transform);
        previousButtonObj.transform.localPosition = previousButtonPos;
        TMP_Text labelTwo = previousButtonObj.GetComponentInChildren<TMP_Text>();
        labelTwo.text = "Previous";
        labelTwo.fontSize = 1;

        Button previousButton = previousButtonObj.GetComponent<Button>();
        previousButton.name = "Previous";
        previousButton.onClick.AddListener(() => btnController.OnButtonClicked(previousButtonObj));
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

    public void CreateMenuKociembaButton()
    {
        Vector3Int buttonPos = new Vector3Int(0, 0, -2);
        GameObject buttonObj = Instantiate(buttonPrefab, uiContainer.transform);
        RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(8,2);
        buttonObj.transform.localPosition = buttonPos;
        TMP_Text labelTwo = buttonObj.GetComponentInChildren<TMP_Text>();
        labelTwo.text = "Fast(Kociemba)";
        RectTransform textTransform = labelTwo.GetComponent<RectTransform>();
        textTransform.sizeDelta = new Vector2(8, 1);
        labelTwo.fontSize = 1;

        //asign event
        Button solveButton = buttonObj.GetComponent<Button>();
        solveButton.name = "Solve_With_Kociemba";
        solveButton.onClick.AddListener(() => btnController.OnButtonClicked(buttonObj));
    }


    public void CreateMenu()
    {
        uiContainer = Instantiate(canvasPrefab, transform);
        RectTransform canvasTransform= uiContainer.GetComponent<RectTransform>();
        canvasTransform.sizeDelta = new Vector2(20, 20);

        var click = uiContainer.AddComponent<ButtonController>();
        click.buttonManager = this;
        CreateMenuKociembaButton();
    }

    void Start()
    {

        CreateMenu();
    }

    void Update()
    {


    }
}
