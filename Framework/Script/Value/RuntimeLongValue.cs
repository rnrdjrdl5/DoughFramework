using System;
using UnityEngine;

[Serializable]
public class RuntimeLongValue
{
    [SerializeField] private long serializedValue;
    private bool hasOverride;
    private long overrideValue;

    public long Value
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
