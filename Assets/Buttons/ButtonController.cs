using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Camera povCamera;
    public GameObject buttonprefab;


    public string nextText = "Next";
    public string previousText = "Previous";

    //Add two buttons next and previous
    public void CreateButtons()
    {
        Vector3Int nextButtonPos = new Vector3Int(3, -2, -5);
        GameObject nextButtonObj = Instantiate(buttonprefab, transform);
        nextButtonObj.transform.position = nextButtonPos;
        nextButtonObj.transform.localScale *= 1.01f;
        TMP_Text labelOne = nextButtonObj.GetComponentInChildren<TMP_Text>();
        labelOne.text = nextText;
        labelOne.fontSize = 1;

        Vector3Int previousButtonPos = new Vector3Int(-3, -2, -5);
        GameObject previousButtonObj = Instantiate(buttonprefab, transform);
        previousButtonObj.transform.position = previousButtonPos;
        TMP_Text labelTwo = previousButtonObj.GetComponentInChildren<TMP_Text>();
        labelTwo.text = previousText;
        labelTwo.fontSize = 1;

    }

    void Start()
    {
        CreateButtons();
    }

    void Update()
    {


    }
}
