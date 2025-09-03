using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject canvasPrefab;
    public GameObject buttonPrefab;
    public GameObject colorButtonPrefab;

    public ButtonController btnController;
    private GameObject uiContainer;
    public GameObject colorPicker;

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

    public void CreateColorPanel()
    {
        colorPicker = new GameObject("btnContainer", typeof(RectTransform));
        colorPicker.transform.localPosition = new Vector3(10f, 0f, -2f);
        colorPicker.transform.SetParent(uiContainer.transform, false);
        RectTransform rectTransform = colorPicker.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(4f, 6f);

        CreateColorPanelBtn(Color.white, new Vector3(-1f, 2f, 0f), "white");
        CreateColorPanelBtn(Color.yellow, new Vector3(1f, 2f, 0f), "yellow");
        CreateColorPanelBtn(Color.red, new Vector3(-1f, 0f, 0f), "red");
        CreateColorPanelBtn(Color.orange, new Vector3(1f, 0f, 0f), "orange");
        CreateColorPanelBtn(Color.green, new Vector3(-1f, -2f, 0f), "green");
        CreateColorPanelBtn(Color.blue, new Vector3(1f, -2f, 0f), "blue");
        
        colorPicker.SetActive(false);
    }

    public void CreateColorPanelBtn(Color color,Vector3 pos,string name)
    {
        GameObject button = Instantiate(colorButtonPrefab, colorPicker.transform);
        button.transform.localPosition = pos;
        Image image = button.GetComponent<Image>();
        image.color = color;
        button.name = name;

        Button buttonObj = button.GetComponent<Button>();
        buttonObj.name = $"colorpicker_{name}";
        buttonObj.onClick.AddListener(() => btnController.ColorPicked(name));
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
        CreateColorPanel();
    }

    void Update()
    {


    }
}
