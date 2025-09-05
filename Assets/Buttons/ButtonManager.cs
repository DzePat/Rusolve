using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject colorButtonPrefab;

    public ButtonController CreateButton(GameObject uiContainer,Vector3 position, Vector2 size,float fontSize,string name,Color color)
    {
        GameObject buttonObj = Instantiate(buttonPrefab, uiContainer.transform);
        buttonObj.transform.localPosition = position;
        RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
        label.text = name;
        label.fontSize = fontSize;
        RectTransform textTransform = label.GetComponent<RectTransform>();
        textTransform.sizeDelta = size;

        Button button = buttonObj.GetComponent<Button>();
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.highlightedColor = color; 
        cb.pressedColor = color * 0.9f;    
        cb.selectedColor = color;
        button.colors = cb;
        button.name = name;
        ButtonController btnController = buttonObj.GetComponent<ButtonController>();
        return btnController;
    }

    public ButtonController CreateColorPanelBtn(GameObject uiContainer, Color color, Vector3 pos, string name)
    {
        GameObject button = Instantiate(colorButtonPrefab, uiContainer.transform);
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
