using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public void Init(string text, Color colour)
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();

        _text.text = text;
        _text.color = colour;
    }
}
