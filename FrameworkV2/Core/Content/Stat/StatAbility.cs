using System;
using System.Collections.Generic;

// 스탯 값을 관리하는 Ability
public sealed class StatAbility : Ability
{
    public event Action<StatChangedPayload> OnChangedStat;

    IStatDefinitionProvider provider;
    DefinitionValueStore<StatDefinition> store;

    // 스탯 정의 공급자를 주입한다
    public void Configure(IStatDefinitionProvider provider)
    {
        this.provider = provider;
        store = provider == null
            ? null
            : new DefinitionValueStore<StatDefinition>(
                provider,
                definition => definition.DefaultValue,
                definition => definition.MinValue,
                definition => definition.MaxValue);
    }

    // 스탯 값을 조회한다
    public bool TryGetValue(int id, out int value)
    {
        value = 0;
        return store != null && store.TryGetValue(id, out value);
    }

    // 스탯 값을 없으면 0으로 반환한다
    public int GetValueOrZero(int id)
    {
        return store != null ? store.GetValueOrZero(id) : 0;
    }

    // 스탯 값을 지정값으로 설정한다
    public bool TrySetValue(int id, int newValue)
    {
        if (store == null || provider == null || !provider.TryResolveId(id, out var resolvedId))
        {
            return false;
        }

        if (!store.TrySetValue(resolvedId, newValue, out var previous, out var delta, out var current))
        {
            return false;
        }

        if (previous == current)
        {
            return true;
        }

        EmitChanged(resolvedId, previous, delta, current);
        return true;
    }

    // 스탯 값을 증감한다
    public bool TryAddValue(int id, int delta)
    {
        if (store == null || provider == null || !provider.TryResolveId(id, out var resolvedId))
        {
            return false;
        }

        if (!store.TryAddValue(resolvedId, delta, out var previous, out var current))
        {
            return false;
        }

        if (previous == current)
        {
            return true;
        }

        EmitChanged(resolvedId, previous, current - previous, current);
        return true;
    }

    // 전체 값 사본을 반환한다
    public IReadOnlyDictionary<int, int> GetValues()
    {
        return store != null ? store.GetValues() : new Dictionary<int, int>();
    }

    void EmitChanged(int id, int previous, int delta, int current)
    {
        OnChangedStat?.Invoke(new StatChangedPayload(id, previous, delta, current));
    }
}
