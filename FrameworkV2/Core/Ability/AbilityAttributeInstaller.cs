using System;

// AbilityAttribute로 선언된 Ability를 자동 부착합니다.
public static class AbilityAttributeInstaller
{
    // 호스트 타입의 AbilityAttribute를 검사해 Ability를 부착합니다.
    public static void Apply(AbilityHost host, Action<Ability> add)
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
