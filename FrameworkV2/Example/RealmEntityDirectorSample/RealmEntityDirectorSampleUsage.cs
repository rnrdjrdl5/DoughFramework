using UnityEngine;

public static class RealmEntityDirectorSampleUsage
{
    public static void Run()
    {
        var realm = CreateRealm("SampleRealm");
        var entity = CreateEntity(realm, "SampleEntity");
        AddEventAbility(entity);
        AttachDirector(entity);
        RefreshSpawn(realm);
    }

    static Realm CreateRealm(string name)
    {
        var realmObject = new GameObject(name);
        var realm = realmObject.AddComponent<Realm>();
        realm.Initialize();
        realm.Ready();
        return realm;
    }

    static Entity CreateEntity(Realm realm, string name)
    {
        var entityObject = new GameObject(name);
        entityObject.transform.SetParent(realm.transform, false);
        var entity = entityObject.AddComponent<Entity>();
        entity.Initialize();
        entity.Ready();
        return entity;
    }

    static EventAbility AddEventAbility(Entity entity)
    {
        return entity.AddAbility<EventAbility>();
    }

    static SampleDirector AttachDirector(Entity entity)
    {
        var director = entity.gameObject.AddComponent<SampleDirector>();
        director.Initialize();
        director.Ready();
        return director;
    }

    static void RefreshSpawn(Realm realm)
    {
        realm.GetAbility<SpawnEntityAbility>()?.RefreshEntities();
    }
}
