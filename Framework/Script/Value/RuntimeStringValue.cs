using System;
using UnityEngine;

[Serializable]
public class RuntimeStringValue
{
    [SerializeField] private string serializedValue;
    private bool hasOverride;
    private string overrideValue;

    public string Value
    {
        get => hasOverride ? overrideValue : serializedValue;
        set
        {
            overrideValue = value;
            hasOverride = true;
        }
    }

    public bool HasOverride => hasOverride;

    public void ClearOverride()
    {
        hasOverride = false;
        overrideValue = default;
    }
}
