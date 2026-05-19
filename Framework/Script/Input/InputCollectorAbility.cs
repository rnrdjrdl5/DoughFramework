using System;
using System.Collections.Generic;
using UnityEngine;

public class InputCollectorAbility : Ability
{
    static readonly KeyCode[] KeyCodes = ResolveKeyCodes();

    InputProcessorAbility inputProcessorAbility;
    Vector2 axis;

    public override void Ready()
    {
        base.Ready();

        inputProcessorAbility = GetInputProcessorAbility();
    }

    public override void Uninitialize()
    {
        inputProcessorAbility = null;
        axis = Vector2.zero;

        base.Uninitialize();
    }

    void Update()
    {
        if (inputProcessorAbility == null)
        {
            return;
        }

        PublishAxisInput();

        foreach (var keyCode in KeyCodes)
        {
            PublishKeyInput(keyCode);
        }
    }

    InputProcessorAbility GetInputProcessorAbility()
    {
        return Entity.GetAbility<InputProcessorAbility>();
    }

    void PublishAxisInput()
    {
        var currentAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (currentAxis == axis && currentAxis == Vector2.zero)
        {
            return;
        }

        axis = currentAxis;
        var stateType = axis == Vector2.zero ? InputStateType.Released : InputStateType.Held;
        inputProcessorAbility.ProcessInput(new InputContext(KeyCode.None, stateType, axis, Input.mousePosition));
    }

    void PublishKeyInput(KeyCode keyCode)
    {
        var stateType = InputStateType.None;
        if (Input.GetKeyDown(keyCode))
        {
            stateType = InputStateType.Pressed;
        }
        else if (Input.GetKeyUp(keyCode))
        {
            stateType = InputStateType.Released;
        }
        else if (Input.GetKey(keyCode))
        {
            stateType = InputStateType.Held;
        }

        if (stateType == InputStateType.None)
        {
            return;
        }

        inputProcessorAbility.ProcessInput(new InputContext(keyCode, stateType, axis, Input.mousePosition));
    }

    static KeyCode[] ResolveKeyCodes()
    {
        var keyCodes = new List<KeyCode>();
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (keyCode == KeyCode.None)
            {
                continue;
            }

            keyCodes.Add(keyCode);
        }

        return keyCodes.ToArray();
    }
}
