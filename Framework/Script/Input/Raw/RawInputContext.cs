using UnityEngine;

public readonly struct RawInputContext
{
    public readonly KeyCode KeyCode;
    public readonly RawInputType InputType;
    public readonly Vector2 Axis;
    public readonly Vector3 ScreenPosition;

    public RawInputContext(KeyCode keyCode, RawInputType inputType, Vector2 axis, Vector3 screenPosition)
    {
        KeyCode = keyCode;
        InputType = inputType;
        Axis = axis;
        ScreenPosition = screenPosition;
    }
}
