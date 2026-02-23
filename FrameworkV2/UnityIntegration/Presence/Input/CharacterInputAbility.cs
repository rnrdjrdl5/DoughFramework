using System;
using UnityEngine;

// 캐릭터 입력(이동 축)을 제공하는 Ability
public sealed class CharacterInputAbility : Ability, IAbilityTick
{
    public float InputX { get; private set; }
    public float InputY { get; private set; }

    public event Action<float, float> MoveInputChanged;

    protected override void OnReady()
    {
        if (AbilityResolver is AbilityHost host)
        {
            host.RegisterTick(this);
        }
    }

    protected override void OnUninitialize()
    {
        if (AbilityResolver is AbilityHost host)
        {
            host.UnregisterTick(this);
        }

        ClearInput();
    }

    public void ClearInput()
    {
        SetInput(0f, 0f);
    }

    public void Tick()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        SetInput(x, y);
    }

    void SetInput(float x, float y)
    {
        if (Mathf.Approximately(InputX, x) && Mathf.Approximately(InputY, y))
        {
            return;
        }

        InputX = x;
        InputY = y;
        try { MoveInputChanged?.Invoke(InputX, InputY); } catch { }
    }
}
