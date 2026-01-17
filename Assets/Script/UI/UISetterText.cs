using System;
using TMPro;
using UnityEngine;

public class UISetterText : UISetter
{
    [SerializeField] TMP_Text targetText;

    public void UpdateText(string text)
    {
        SetText(targetText, text);
    }
}
