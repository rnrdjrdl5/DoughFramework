using UnityEngine;

// 기본 Realm을 생성하는 표준 빌더
public class CommonBuilder : RealmBuilder
{
    // 기본 Realm 인스턴스를 생성합니다.
    public override Realm Build(Realm parent)
    {
        // Create and configure the new Realm instance.
        // Name policy: builder decides; duplicates are allowed.
        var realmObject = new GameObject("Realm");
        var realm = realmObject.AddComponent<Realm>();

        // Configure realm here (aliases/abilities/entities) as needed.
        // e.g., realm.AddAlias("ingame");
        // e.g., realm.AddAbility<SomeRealmAbility>();

        return realm;
    }
}
