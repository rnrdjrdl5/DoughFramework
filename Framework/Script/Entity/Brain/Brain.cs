using System;
using System.Collections.Generic;
using UnityEngine;

public class Brain : Entity
{
    public static string PrefabPath = "Player/PlayerBrain";
    public event Action<IControlled> OnAttachControll;
    public event Action<IControlled> OnDetachControll;
    public event Action<bool> OnChangedAI;
    public IControlled Controll => controlled;
    public bool IsAI => isAI;
    
    IControlled controlled;
    
    // NOTE : 추후 int값으로 유저고유값으로 전환시키기 ( 멀티 대응 )
    bool isAI;

    public void AttachControll(IControlled controlled)
    {
        this.controlled = controlled;
        OnAttachControll?.Invoke(controlled);
    }

    public void DetachControll()
    {
        OnDetachControll?.Invoke(controlled);
        controlled = null;
    }

    public void SetAI(bool isAI)
    {
        this.isAI = isAI;
        OnChangedAI?.Invoke(isAI);
    }
}