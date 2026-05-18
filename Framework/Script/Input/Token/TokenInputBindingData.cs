using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class TokenInputBindingData : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    [JsonProperty] Dictionary<KeyCode, TokenInputType> inputTypeByKeyCode = new();

    public void Initialize(IInitData initData = null)
    {
        inputTypeByKeyCode.Clear();

        SetInputType(KeyCode.Mouse0, TokenInputType.PointerPrimary);
        SetInputType(KeyCode.Mouse1, TokenInputType.PointerSecondary);
        SetInputType(KeyCode.Z, TokenInputType.Action1);
        SetInputType(KeyCode.Q, TokenInputType.Action2);
        SetInputType(KeyCode.E, TokenInputType.Action3);
        SetInputType(KeyCode.R, TokenInputType.Action4);
        SetInputType(KeyCode.Space, TokenInputType.Action5);
        SetInputType(KeyCode.LeftShift, TokenInputType.Action6);
        SetInputType(KeyCode.F1, TokenInputType.Menu1);
        SetInputType(KeyCode.F2, TokenInputType.Menu2);
        SetInputType(KeyCode.I, TokenInputType.Menu3);
        SetInputType(KeyCode.Escape, TokenInputType.Cancel);
    }

    public void Uninitialize()
    {
        inputTypeByKeyCode.Clear();
    }

    public void OnSetMessageBus()
    {
    }

    public void SetInputType(KeyCode keyCode, TokenInputType inputType)
    {
        inputTypeByKeyCode[keyCode] = inputType;
    }

    public bool TryGetInputType(RawInputContext rawInput, out TokenInputType inputType)
    {
        if (rawInput.KeyCode == KeyCode.None)
        {
            inputType = TokenInputType.MoveAxis;
            return true;
        }

        return inputTypeByKeyCode.TryGetValue(rawInput.KeyCode, out inputType);
    }

    public IEnumerable<KeyCode> GetBoundKeyCodes()
    {
        return inputTypeByKeyCode.Keys;
    }
}
