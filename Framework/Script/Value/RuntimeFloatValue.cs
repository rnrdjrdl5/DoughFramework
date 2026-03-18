using System;
using UnityEngine;

[Serializable]
public class RuntimeFloatValue
{
    [SerializeField] private float serializedValue;
    private bool hasOverride;
    private float overrideValue;

    public float Value
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
