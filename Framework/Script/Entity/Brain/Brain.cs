using System;
using System.Collections.Generic;
using UnityEngine;

public enum BrainControlMode
{
    PlayerInput,
    AI,
}

public class Brain : Entity
{
    public static string PrefabPath = "Player/PlayerBrain";
    public event Action<IControlled> OnAttachControll;
    public event Action<IControlled> OnDetachControll;
    public event Action<BrainControlMode> OnChangedControlMode;
    public IControlled Controll => controlled;
    public BrainControlMode ControlMode => controlMode;
    
    IControlled controlled;
    
    // NOTE : 추후 int값으로 유저고유값으로 전환시키기 ( 멀티 대응 )
    BrainControlMode controlMode = BrainControlMode.PlayerInput;

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

    public void SetControlMode(BrainControlMode controlMode)
    {
        this.controlMode = controlMode;
        OnChangedControlMode?.Invoke(controlMode);
    }
}
