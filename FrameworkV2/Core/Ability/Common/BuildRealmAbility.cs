using System;

public class BuildRealmAbility : Ability
{
    // 새 Realm이 빌드되어 부모에 부착된 직후 알림
    public event Action<Realm> RealmBuilt;
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

    public Realm Build<TBuilder>(Realm owner) where TBuilder : RealmBuilder, new()
    {
        var builder = new TBuilder();
        return Build(owner, builder);
    }

    protected virtual void OnRealmBuilt(Realm realm)
    {
    }
}
