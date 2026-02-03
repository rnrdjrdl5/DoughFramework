using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class AbilityAttributeCache
{
    static readonly Dictionary<Type, Type[]> cache = new();

    public static Type[] GetAbilityTypes(Type hostType)
    {
        if (hostType == null)
        {
            return Array.Empty<Type>();
        }

        if (cache.TryGetValue(hostType, out var cached))
        {
            return cached;
        }

        var dict = new Dictionary<Type, int>();

        var attrs = hostType.GetCustomAttributes<AbilityAttribute>(true).ToArray();
        foreach (var attr in attrs)
        {
            foreach (var t in attr.AbilityTypes)
            {
                if (t == null)
                {
                    continue;
                }

                if (!typeof(Ability).IsAssignableFrom(t))
                {
                    continue;
                }

                if (dict.TryGetValue(t, out var existing))
                {
                    dict[t] = Math.Min(existing, attr.Order);
                }
                else
                {
                    dict[t] = attr.Order;
                }
            }
        }

        var ordered = dict
            .OrderBy(kv => kv.Value)
            .Select(kv => kv.Key)
            .ToArray();

        cache[hostType] = ordered;
        return ordered;
    }

    public static void Clear()
    {
        cache.Clear();
    }
}

