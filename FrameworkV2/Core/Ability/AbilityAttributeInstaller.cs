using System;
using System.Reflection;

public static class AbilityAttributeInstaller
{
    public static void Apply(object host, Action<Ability> add)
    {
        if (host == null || add == null)
        {
            return;
        }

        var types = AbilityAttributeCache.GetAbilityTypes(host.GetType());
        for (int i = 0; i < types.Length; i++)
        {
            var t = types[i];
            var ctor = t.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                continue;
            }

            var instance = (Ability)Activator.CreateInstance(t);
            if (instance == null)
            {
                continue;
            }

            add(instance);
        }
    }
}

