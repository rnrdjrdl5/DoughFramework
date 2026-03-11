using System;
using UnityEngine;

public class HpAbility : Ability
{
    public event Action<float,float,float> OnChangeHp;
    public float Hp => hp;
    
    float hp;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
    }

    public bool TryApplyDamage(Entity caster, float damage)
    {
        if (damage < 0)
        {
            return false;
        }

        var prevHp = hp;
        hp = Mathf.Max(0, hp - damage);
        
        OnChangeHp?.Invoke(prevHp, hp, damage);

        return true;
    }

    public void Heal(Entity caster, float point)
    {
        var prevHp = hp;
        hp += point;
        
        OnChangeHp?.Invoke(prevHp, hp, point);
    }

    public void SetHp(float changedHp, bool withoutEvent = true)
    {
        var originHp = hp;
        hp = changedHp;
        
        if (!withoutEvent)
        {
            OnChangeHp?.Invoke(originHp, hp, Mathf.Abs(hp - originHp));
        }
    }
}
