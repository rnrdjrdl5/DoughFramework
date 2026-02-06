using System;
using System.Collections.Generic;

public sealed class DefinitionValueStore<TDefinition>
{
    readonly IDefinitionProvider<TDefinition> provider;
    readonly Func<TDefinition, int> getDefaultValue;
    readonly Func<TDefinition, int?> getMinValue;
    readonly Func<TDefinition, int?> getMaxValue;
    readonly Dictionary<int, int> values = new();

    public DefinitionValueStore(
        IDefinitionProvider<TDefinition> provider,
        Func<TDefinition, int> getDefaultValue,
        Func<TDefinition, int?> getMinValue = null,
        Func<TDefinition, int?> getMaxValue = null)
    {
        this.provider = provider;
        this.getDefaultValue = getDefaultValue ?? throw new ArgumentNullException(nameof(getDefaultValue));
        this.getMinValue = getMinValue;
        this.getMaxValue = getMaxValue;
    }

    public bool TryGetValue(int id, out int value)
    {
        value = 0;
        if (!TryResolveDefinition(id, out var resolvedId, out var definition))
        {
            return false;
        }

        value = values.TryGetValue(resolvedId, out var current) ? current : getDefaultValue(definition);
        return true;
    }

    public int GetValueOrZero(int id)
    {
        return TryGetValue(id, out var value) ? value : 0;
    }

    public bool TrySetValue(int id, int newValue, out int previous, out int delta, out int current)
    {
        previous = 0;
        delta = 0;
        current = 0;

        if (!TryResolveDefinition(id, out var resolvedId, out var definition))
        {
            return false;
        }

        if (!IsWithinRange(newValue, definition))
        {
            return false;
        }

        previous = values.TryGetValue(resolvedId, out var existing) ? existing : getDefaultValue(definition);
        if (previous == newValue)
        {
            current = previous;
            delta = 0;
            return true;
        }

        values[resolvedId] = newValue;
        current = newValue;
        delta = newValue - previous;
        return true;
    }

    public bool TryAddValue(int id, int delta, out int previous, out int current)
    {
        previous = 0;
        current = 0;

        if (!TryResolveDefinition(id, out var resolvedId, out var definition))
        {
            return false;
        }

        previous = values.TryGetValue(resolvedId, out var existing) ? existing : getDefaultValue(definition);
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
            current = previous;
            return true;
        }

        values[resolvedId] = next;
        current = next;
        return true;
    }

    public IReadOnlyDictionary<int, int> GetValues()
    {
        return new Dictionary<int, int>(values);
    }

    bool TryResolveDefinition(int id, out int resolvedId, out TDefinition definition)
    {
        resolvedId = 0;
        definition = default;

        if (provider == null)
        {
            return false;
        }

        return provider.TryResolveId(id, out resolvedId) && provider.TryGetDefinition(resolvedId, out definition);
    }

    bool IsWithinRange(int value, TDefinition definition)
    {
        if (getMinValue != null)
        {
            var minValue = getMinValue(definition);
            if (minValue.HasValue && value < minValue.Value)
            {
                return false;
            }
        }

        if (getMaxValue != null)
        {
            var maxValue = getMaxValue(definition);
            if (maxValue.HasValue && value > maxValue.Value)
            {
                return false;
            }
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
}
