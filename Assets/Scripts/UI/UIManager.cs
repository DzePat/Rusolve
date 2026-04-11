using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject counterPrefab;
    public GameObject canvasPrefab;
    public GameObject popupPrefab;
    public GameObject colorPanel;
    public GameObject sidePanel;
    private bool popupactive;

    private GameObject colorStatistics;
    public Dictionary <string, TMP_Text> colorsStats = new();
    public GameObject uiContainer;


    /// <summary>
    /// Adds a 1 to the chosen color in color statistics list 
    /// </summary>
    /// <param name="color"></param>
    public void CountAdd(string color)
    {
        TMP_Text obj = colorsStats[color];
        obj.text = "" + (int.Parse(obj.text) + 1);
    }

    /// <summary>
    /// Subtracts a 1 from the chosen color in color statistics list 
    /// </summary>
    /// <param name="color"></param>
    public void CountSub(string color)
    {
        TMP_Text obj = colorsStats[color];
        obj.text = "" + (int.Parse(obj.text) - 1);
    }

    /// <summary>
    /// Resets all color counts to 9 in color statistics list.
    /// </summary>
    public void CountReset()
    {
        foreach (var key in colorsStats.Keys.ToList())
        {
            colorsStats[key].text = "9";

        }
    }

    public void CreateUiContainer()
    {
        uiContainer = Instantiate(canvasPrefab, transform);
        RectTransform canvasTransform = uiContainer.GetComponent<RectTransform>();
        canvasTransform.sizeDelta = new Vector2(20, 20);
    }

  

    public void CreateColorStatistics()
    {
        colorStatistics = new GameObject("colorStatistics");
        colorStatistics.transform.localPosition = new Vector3(0f, 4f, -2f);
        colorStatistics.transform.SetParent(uiContainer.transform, false);

        CreateColorStat("white",Color.white, -2.5f);
        CreateColorStat("yellow",Color.yellow, -1.5f);
        CreateColorStat("red" ,Color.red, -0.5f);
        CreateColorStat("orange",Color.orange,0.5f);
        CreateColorStat("green",Color.green, 1.5f);
        CreateColorStat("blue",Color.blue,2.5f);

        colorStatistics.SetActive(false);
    }

    private void CreateColorStat(string name,Color color,float xpos)
    {
        GameObject colorStat = Instantiate(counterPrefab, colorStatistics.transform);
        colorStat.transform.localPosition =new Vector3 (xpos,0,0);
        RawImage colorimage = colorStat.GetComponent<RawImage>();
        colorimage.color = color;
        TMP_Text textNumber = colorStat.GetComponentInChildren<TMP_Text>();
        textNumber.text = "9";
        textNumber.fontSize = 0.5f;
        colorsStats[name] = textNumber;
    }

    public void CreateColorPanel()
    {
        colorPanel = new GameObject("btnContainer", typeof(RectTransform));
        colorPanel.transform.localPosition = new Vector3(-7.5f, 0f, -2f);
        colorPanel.transform.SetParent(uiContainer.transform, false);
        RectTransform rectTransform = colorPanel.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(4f, 10f);

        colorPanel.SetActive(false);
    }

    public void CreateSidePanel()
    {
        sidePanel = new GameObject("btnContainer", typeof(RectTransform));
        sidePanel.transform.localPosition = new Vector3(7.5f, 0f, -2f);
        sidePanel.transform.SetParent(uiContainer.transform, false);

        GameObject rotContainer = new GameObject("rotContainer", typeof(RectTransform));
        rotContainer.transform.SetParent(sidePanel.transform, false);

        sidePanel.SetActive(false);
    }

    public void ShowSidePanelRotationButtons()
    {
        Transform rotContainer = sidePanel.transform.Find("rotContainer");
        rotContainer.gameObject.SetActive(true);
    }
    public void HideSidePanelRotationButtons()
    {
        Transform rotContainer = sidePanel.transform.Find("rotContainer");
        rotContainer.gameObject.SetActive(false);
    }

    public void ShowPopup(string message, Vector3 position)
    {
        if (popupactive != true)
        {
            GameObject popup = Instantiate(popupPrefab, uiContainer.transform);
            popup.transform.localPosition = position;

            TMP_Text text = popup.GetComponentInChildren<TMP_Text>();
            if (text != null) text.text = message;
            popupactive = true;
            StartCoroutine(HideAfterSeconds(popup, 2f));
        }
    }

    private IEnumerator HideAfterSeconds(GameObject popup, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(popup);
        popupactive = false;
    }

    public void ShowStatistics()
    {
        colorStatistics.SetActive(true);

    }

    public void HideStatistics()
    {
        colorStatistics.SetActive(false);
    }

    void LateUpdate()
    {
        uiContainer.transform.rotation = Camera.main.transform.rotation;
        Vector3 offset = new (0, 0, 10);
        uiContainer.transform.position = Camera.main.transform.position + Camera.main.transform.rotation * offset;
    }
}
