using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject colorButtonPrefab;
    public UIManager uiManager;

    public ButtonController btnController;
    public GameObject colorPicker;

    //Add two buttons next and previous
    public void CreateSolveNavigationButtons()
    {
        Vector3Int nextButtonPos = new (3, -3, -2);
        GameObject nextButtonObj = Instantiate(buttonPrefab,uiManager.uiContainer.transform);
        nextButtonObj.transform.localPosition = nextButtonPos;
        TMP_Text labelOne = nextButtonObj.GetComponentInChildren<TMP_Text>();
        nextButtonObj.name = "Next";
        labelOne.text = "Next";
        labelOne.fontSize = 1;

        Button nextButton = nextButtonObj.GetComponent<Button>();
        nextButton.name = "Next";
        nextButton.onClick.AddListener(() => btnController.OnButtonClicked(nextButtonObj));

        Vector3Int previousButtonPos = new (-3, -3, -2);
        GameObject previousButtonObj = Instantiate(buttonPrefab, uiManager.uiContainer.transform);
        previousButtonObj.transform.localPosition = previousButtonPos;
        TMP_Text labelTwo = previousButtonObj.GetComponentInChildren<TMP_Text>();
        labelTwo.text = "Previous";
        labelTwo.fontSize = 1;

        Button previousButton = previousButtonObj.GetComponent<Button>();
        previousButton.name = "Previous";
        previousButton.onClick.AddListener(() => btnController.OnButtonClicked(previousButtonObj));
    }

    public void CreateSolveButton()
    {
        Vector3Int buttonPos = new (0, -3, -2);
        GameObject buttonObj = Instantiate(buttonPrefab, uiManager.uiContainer.transform);
        buttonObj.transform.localPosition = buttonPos;
        RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(4, 2);
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
        Vector3Int buttonPos = new (0, 0, -2);
        GameObject buttonObj = Instantiate(buttonPrefab, uiManager.uiContainer.transform);
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
        colorPicker.transform.localPosition = new Vector3(7.5f, 0f, -2f);
        colorPicker.transform.SetParent(uiManager.uiContainer.transform, false);
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

    

}
