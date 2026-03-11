using System;
using UnityEngine;

public class HpAbility : Ability
{
    public event Action<float,float> OnChangeHp;
    public float Hp => hp;

    float maxHp;
    float hp;

    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
    }

    public void SetMaxHp(float maxHp, bool fillHp = true)
    {
        this.maxHp = maxHp;
        if (fillHp)
        {
            var prevHp = hp;
            hp = maxHp;
            OnChangeHp?.Invoke(prevHp, hp);
        }
    }

    public bool TryApplyDamage(Entity caster, float damage)
    {
        if (damage < 0)
        {
            return false;
        }

        var prevHp = hp;
        hp = Mathf.Max(0, hp - damage);
        
        OnChangeHp?.Invoke(prevHp, hp);

        return true;
    }

    public void Heal(Entity caster, float point)
    {
        var prevHp = hp;
        hp += point;
        
        OnChangeHp?.Invoke(prevHp, hp);
    }

    public void SetHp(float changedHp, bool withoutEvent = true)
    {
        var originHp = hp;
        hp = changedHp;
        
        if (!withoutEvent)
        {
            OnChangeHp?.Invoke(originHp, hp);
        }
    }
}
