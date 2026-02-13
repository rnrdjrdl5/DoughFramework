using UnityEngine;

// Ability 사용 예시
public static class ExampleAbilityComponentUsage
{
    // AbilityHost에 Ability를 부착하고 수명을 호출하는 흐름을 보여줍니다.
    public static void Run()
    {
        var realmObject = new GameObject("AbilityExampleRealm");
        var realm = realmObject.AddComponent<Realm>();

        var ability = realm.AddAbility<SpawnEntityAbility>();

        realm.Initialize();
        realm.Ready();

        ability.RefreshEntities();
    }
}
