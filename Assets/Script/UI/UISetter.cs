using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UISetter : MonoBehaviour
{
    protected void SetImage(Image image, Sprite sprite)
    {
        if (image == null || sprite == null)
        {
            return;
        }
        
        image.sprite = sprite;
    }

    protected void SetText(TMP_Text text, string str)
    {
        if (text == null || string.IsNullOrEmpty(str))
        {
            return;
        }
        
        text.text = str;
    }
}
