using UnityEngine;

// Realm/Entity/Ability 컴포넌트 구조의 간단 예시
public static class EntityExampleUsage
{
    // 간단한 Realm/Entity 생성 흐름을 보여줍니다.
    public static void Run()
    {
        var realmObject = new GameObject("ExampleRealm");
        var realm = realmObject.AddComponent<Realm>();
        realm.Initialize();
        realm.Ready();

        var entityObject = new GameObject("ExampleEntity");
        entityObject.transform.SetParent(realmObject.transform, false);
        var entity = entityObject.AddComponent<Entity>();
        entity.Initialize();
        entity.Ready();

        var spawn = realm.GetAbility<SpawnEntityAbility>();
        spawn?.RefreshEntities();
    }
}
