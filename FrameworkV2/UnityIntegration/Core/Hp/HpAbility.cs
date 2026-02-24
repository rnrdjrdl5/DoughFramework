using System;

public sealed class HpAbility : Ability
{
    public int Max { get; private set; }
    public int Current { get; private set; }

    public event Action<int, int> HpChanged;

    public void SetMax(int max)
    {
        if (max < 0)
        {
            max = 0;
        }

        if (Max == max)
        {
            return;
        }

        Max = max;

        if (Current > Max)
        {
            SetCurrent(Max);
        }
    }

    public void SetCurrent(int current)
    {
        if (current < 0)
        {
            current = 0;
        }

        if (current > Max)
        {
            current = Max;
        }

        if (Current == current)
        {
            return;
        }

        var previous = Current;
        Current = current;
        try { HpChanged?.Invoke(previous, Current); } catch { }
    }

    public void Apply(int damage)
    {
        SetCurrent(Current - damage);
    }

    public void Heal(int damage)
    {
        SetCurrent(Current + damage);
    }
}
