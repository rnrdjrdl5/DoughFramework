using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PhysicalInputBindingData : IEntityData, IMessageBus
{
    [JsonIgnore] public MessageBus MessageBus { get; set; }

    [JsonProperty] Dictionary<KeyCode, PhysicalInputTokenType> tokenTypeByKeyCode = new();

    public void Initialize(IInitData initData = null)
    {
        tokenTypeByKeyCode.Clear();

        SetTokenType(KeyCode.Mouse0, PhysicalInputTokenType.PrimaryClick);
        SetTokenType(KeyCode.Mouse1, PhysicalInputTokenType.SecondaryClick);
        SetTokenType(KeyCode.Z, PhysicalInputTokenType.Pick);
        SetTokenType(KeyCode.Q, PhysicalInputTokenType.Hotkey1);
        SetTokenType(KeyCode.E, PhysicalInputTokenType.Hotkey2);
        SetTokenType(KeyCode.R, PhysicalInputTokenType.Hotkey3);
        SetTokenType(KeyCode.Space, PhysicalInputTokenType.Utility1);
        SetTokenType(KeyCode.LeftShift, PhysicalInputTokenType.Utility2);
        SetTokenType(KeyCode.F1, PhysicalInputTokenType.ToggleTeam);
        SetTokenType(KeyCode.F2, PhysicalInputTokenType.ToggleEquipment);
        SetTokenType(KeyCode.I, PhysicalInputTokenType.ToggleInventory);
        SetTokenType(KeyCode.Escape, PhysicalInputTokenType.Back);
    }

    public void Uninitialize()
    {
        tokenTypeByKeyCode.Clear();
    }

    public void OnSetMessageBus()
    {
    }

    public void SetTokenType(KeyCode keyCode, PhysicalInputTokenType tokenType)
    {
        tokenTypeByKeyCode[keyCode] = tokenType;
    }

    public bool TryGetTokenType(KeyCode keyCode, out PhysicalInputTokenType tokenType)
    {
        return tokenTypeByKeyCode.TryGetValue(keyCode, out tokenType);
    }

    public IEnumerable<KeyCode> GetBoundKeyCodes()
    {
        return tokenTypeByKeyCode.Keys;
    }
}
