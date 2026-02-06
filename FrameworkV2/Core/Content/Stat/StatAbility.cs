using System;
using System.Collections.Generic;

// 스탯 값을 관리하는 Ability
public sealed class StatAbility : Ability
{
    public event Action<StatChangedPayload> OnChangedStat;

    IStatDefinitionProvider provider;
    readonly Dictionary<int, int> values = new();

    // 스탯 정의 공급자를 주입한다
    public void Configure(IStatDefinitionProvider provider)
    {
        this.provider = provider;
    }

    // 스탯 값을 조회한다
    public bool TryGetValue(int id, out int value)
    {
        value = 0;
        if (!TryResolveDefinition(id, out var resolvedId, out var definition))
        {
            return false;
        }

        value = values.TryGetValue(resolvedId, out var current) ? current : definition.DefaultValue;
        return true;
    }

    // 스탯 값을 없으면 0으로 반환한다
    public int GetValueOrZero(int id)
    {
        return TryGetValue(id, out var value) ? value : 0;
    }

    // 스탯 값을 지정값으로 설정한다
    public bool TrySetValue(int id, int newValue)
    {
        if (!TryResolveDefinition(id, out var resolvedId, out var definition))
        {
            return false;
        }

        if (!IsWithinRange(newValue, definition))
        {
            return false;
        }

        var previous = values.TryGetValue(resolvedId, out var current) ? current : definition.DefaultValue;
        if (previous == newValue)
        {
            return true;
        }

        values[resolvedId] = newValue;
        EmitChanged(resolvedId, previous, newValue - previous, newValue);
        return true;
    }

    // 스탯 값을 증감한다
    public bool TryAddValue(int id, int delta)
    {
        if (!TryResolveDefinition(id, out var resolvedId, out var definition))
        {
            return false;
        }

        var previous = values.TryGetValue(resolvedId, out var current) ? current : definition.DefaultValue;
        if (!TryApplyDelta(previous, delta, out var next))
        {
            return false;
        }

        if (!IsWithinRange(next, definition))
        {
            return false;
        }

        if (previous == next)
        {
            return true;
        }

        values[resolvedId] = next;
        EmitChanged(resolvedId, previous, delta, next);
        return true;
    }

    // 전체 값 사본을 반환한다
    public IReadOnlyDictionary<int, int> GetValues()
    {
        return new Dictionary<int, int>(values);
    }

    bool TryResolveDefinition(int id, out int resolvedId, out StatDefinition definition)
    {
        resolvedId = 0;
        definition = default;

        if (provider == null)
        {
            return false;
        }

        return provider.TryResolveId(id, out resolvedId) && provider.TryGetDefinition(resolvedId, out definition);
    }

    bool IsWithinRange(int value, StatDefinition definition)
    {
        if (definition.MinValue.HasValue && value < definition.MinValue.Value)
        {
            return false;
        }

        if (definition.MaxValue.HasValue && value > definition.MaxValue.Value)
        {
            return false;
        }

        return true;
    }

    bool TryApplyDelta(int current, int delta, out int result)
    {
        result = 0;

        long next = (long)current + delta;
        if (next > int.MaxValue || next < int.MinValue)
        {
            return false;
        }

        result = (int)next;
        return true;
    }

    void EmitChanged(int id, int previous, int delta, int current)
    {
        OnChangedStat?.Invoke(new StatChangedPayload(id, previous, delta, current));
    }
}
