using System;
using UnityEngine;

[Serializable]
public class RuntimeIntValue
{
    [SerializeField] private int serializedValue;
    private bool hasOverride;
    private int overrideValue;

    public int Value
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
