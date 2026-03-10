using System;
using System.Collections.Generic;
using UnityEngine;

public class Brain : Entity
{
    public static string PrefabPath = "Player/PlayerBrain";
    
    public IControlled Controll => controlled;

    public event Action<IControlled> OnAttachControll;
    public event Action<IControlled> OnDetachControll;
    
    IControlled controlled;

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
}