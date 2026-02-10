using UnityEngine;

// StatAbility 사용 예시를 보여주는 참고 스크립트
public static class ExampleStatAbility
{
    // Example: 스탯 정의 등록 -> StatAbility 사용
    public static void RunExample()
    {
        var provider = new StaticStatDefinitionProvider();
        provider.Register("Strength", new StatDefinition
        {
            DefaultValue = 10,
            MinValue = 1,
            MaxValue = 999
        });
        provider.Register("Agility", new StatDefinition
        {
            DefaultValue = 5,
            MinValue = 0,
            MaxValue = 500
        });

        var hostObject = new GameObject("StatHost");
        var host = hostObject.AddComponent<Entity>();
        var statAbility = host.AddAbility<StatAbility>();
        statAbility.Configure(provider);

        host.Initialize();
        host.Ready();

        statAbility.OnChangedStat += payload =>
        {
            // 변경된 스탯을 수신한다
        };

        var strengthId = HashKey.FromName("Strength");
        var agilityId = HashKey.FromName("Agility");

        statAbility.TryAddValue(strengthId, 3);
        statAbility.TrySetValue(agilityId, 12);
        statAbility.TryAddValue(agilityId, -2);
    }

    sealed class StaticStatDefinitionProvider : IStatDefinitionProvider
    {
        readonly System.Collections.Generic.Dictionary<int, StatDefinition> definitions = new();

        // 스탯 정의를 등록합니다.
        public void Register(string name, StatDefinition definition)
        {
            var id = HashKey.FromName(name);
            definitions[id] = definition;
        }

        // 스탯 Id를 해석합니다.
        public bool TryResolveId(int id, out int resolvedId)
        {
            resolvedId = id;
            return definitions.ContainsKey(id);
        }

        // 스탯 정의를 조회합니다.
        public bool TryGetDefinition(int id, out StatDefinition definition)
        {
            return definitions.TryGetValue(id, out definition);
        }
    }
}
