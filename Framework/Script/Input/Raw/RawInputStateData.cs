using Newtonsoft.Json;
using UnityEngine;

public class RawInputStateData : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    public Vector2 AxisDirection { get; private set; }
    public Vector3 MouseScreenPosition { get; private set; }

    public void Initialize(IInitData initData = null)
    {
        AxisDirection = Vector2.zero;
        MouseScreenPosition = Vector3.zero;
    }

    public void Uninitialize()
    {
        AxisDirection = Vector2.zero;
        MouseScreenPosition = Vector3.zero;
    }

    public void OnSetMessageBus()
    {
    }

    public bool SetAxisDirection(Vector2 axisDirection)
    {
        if (AxisDirection == axisDirection)
        {
            return false;
        }

        AxisDirection = axisDirection;
        return true;
    }

    public bool SetMouseScreenPosition(Vector3 mouseScreenPosition)
    {
        if (MouseScreenPosition == mouseScreenPosition)
        {
            return false;
        }

        MouseScreenPosition = mouseScreenPosition;
        return true;
    }
}
