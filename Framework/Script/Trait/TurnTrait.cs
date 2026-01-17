using System;
using Unity.VisualScripting;
using UnityEngine;

// Turn을 제어하는 Trait입니다.

public class TurnTrait : Trait
{
    public bool IsStop { get; private set; }
    public int MaxPoint { get; private set; }
    public int MaxPointOffset { get; set; }
    public int LeftPoint { get; set; }

    public Action OnPlayTurn;
    public Action<TurnTrait> OnEndTurn;

    public void Pause()
    {
        IsStop = true;
    }

    public void Resume()
    {
        IsStop = false;
    }

    public void PlayTurn()
    {
        OnPlayTurn?.Invoke();
    }

    public void EndTurn()
    {
        OnEndTurn?.Invoke(this);
    }

    public void FillPoint()
    {
        LeftPoint = MaxPoint + MaxPointOffset;
    }

    public void SetMaxPoint(int point)
    {
        MaxPoint = point;
        LeftPoint = MaxPoint;
    }
}
