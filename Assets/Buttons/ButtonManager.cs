using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject colorButtonPrefab;


    public ButtonController CreateButton(GameObject uiContainer,Vector3 position, Vector2 size,string name)
    {
        GameObject buttonObj = Instantiate(buttonPrefab, uiContainer.transform);
        buttonObj.transform.localPosition = position;
        RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
        label.text = name;
        label.fontSize = 1;
        RectTransform textTransform = label.GetComponent<RectTransform>();
        textTransform.sizeDelta = size;

        Button button = buttonObj.GetComponent<Button>();
        button.name = name;
        ButtonController btnController = buttonObj.GetComponent<ButtonController>();
        return btnController;
    }

    public ButtonController CreateColorPanelBtn(GameObject colorPanel, Color color, Vector3 pos, string name)
    {
        GameObject button = Instantiate(colorButtonPrefab, colorPanel.transform);
        button.transform.localPosition = pos;

        Image image = button.GetComponent<Image>();
        image.color = color;
        button.name = name;

        Button buttonObj = button.GetComponent<Button>();
        buttonObj.name = $"colorpicker_{name}";
        ButtonController btnController = buttonObj.GetComponent<ButtonController>();

        return btnController;
    }
}
