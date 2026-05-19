using UnityEngine;

public readonly struct InputContext
{
    public readonly KeyCode KeyCode;
    public readonly InputStateType StateType;
    public readonly Vector2 Axis;
    public readonly Vector3 ScreenPosition;

    public InputContext(KeyCode keyCode, InputStateType stateType, Vector2 axis, Vector3 screenPosition)
    {
        KeyCode = keyCode;
        StateType = stateType;
        Axis = axis;
        ScreenPosition = screenPosition;
    }
}
