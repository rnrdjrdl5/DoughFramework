using System;
using UnityEngine;
using UnityEngine.UI;

public class UIEventDispatcher : MonoBehaviour
{
    Action clickEvent;
    
    public void OnClickEvent()
    {
        clickEvent?.Invoke();
    }

    public void SetClickEvent(Action evnt)
    {
        clickEvent = evnt;
    }
}
