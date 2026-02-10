using System;

// Realm 빌드 및 부착 흐름을 제공하는 Ability
public class BuildRealmAbility : Ability
{
    // 새 Realm이 빌드되어 부모에 부착된 직후 알림
    public event Action<Realm> RealmBuilt;

    // RealmBuilder를 이용해 자식 Realm을 생성하고 부착합니다.
    public Realm Build(Realm owner, RealmBuilder builder)
    {
        if (owner == null)
        {
            throw new ArgumentNullException(nameof(owner));
        }
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        var realm = builder.Build(owner);
        if (realm == null)
        {
            throw new InvalidOperationException("RealmBuilder returned null.");
        }

        realm.AttachUpstreamResolver(AbilityResolver);
        owner.AddChild(realm);
        OnRealmBuilt(realm);
        try { RealmBuilt?.Invoke(realm); } catch { }
        return realm;
    }

    // RealmBuilder를 제네릭으로 생성해 자식 Realm을 생성합니다.
    public Realm Build<TBuilder>(Realm owner) where TBuilder : RealmBuilder, new()
    {
        var builder = new TBuilder();
        return Build(owner, builder);
    }

    // 자식 Realm 생성 후 확장 포인트를 제공합니다.
    protected virtual void OnRealmBuilt(Realm realm)
    {
    }
}
