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
    private ButtonManager buttonManager;


    void Awake()
    {
        _button = GetComponentInChildren<Button>();
        if (_button == null)
        {
            Debug.LogError("ButtonController: No Button component found in children!", this);
            return;
        }
        _button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        OnClicked?.Invoke(this);
    }   

}
