using System;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public event Action<ButtonController> OnClicked;
    private Button _button;
    [SerializeField] private ButtonManager buttonManager;
    

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        OnClicked?.Invoke(this);
    }   

}
