using System;
using UnityEngine;

// AbilityAttribute로 선언된 Ability 컴포넌트를 자동 부착합니다.
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
            var existing = host.GetComponent(t) as Ability;
            if (existing != null)
            {
                add(existing);
                continue;
            }

            var instance = host.gameObject.AddComponent(t) as Ability;
            if (instance == null)
            {
                continue;
            }

            add(instance);
        }
    }
}
