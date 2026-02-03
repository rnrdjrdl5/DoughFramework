using UnityEngine;

public class SpawnService : Service, ISpawnService
{
    ObjectPoolService pool;
    IUIService ui;

    public override string ToString()
    {
        return nameof(SpawnService);
    }

    protected override void OnReady()
    {
        // Resolve UI service via interface (registered by ServiceSet)
        ui = Services?.Get<IUIService>();

        // Resolve pool (class is not registered by interface per AGENTS.md) with fallbacks
        if (pool == null)
        {
            pool = Services?.Get<ObjectPoolService>();
            if (pool == null)
            {
                pool = GetComponentInParent<ObjectPoolService>();
            }
            if (pool == null)
            {
                pool = GameObject.FindObjectOfType<ObjectPoolService>();
            }
        }
    }

    // Generic GameObject
    public GameObject Spawn(string key)
    {
        if (string.IsNullOrEmpty(key)) return null;
        var prefab = AddressableAccess.LoadAsset<GameObject>(key);
        if (prefab == null) return null;
        if (pool == null) return GameObject.Instantiate(prefab);
        return pool.Spawn(prefab);
    }

    // UI helpers
    public UI SpawnUIToCore(string key, Realm realm = null)
    {
        var go = Spawn(key);
        if (go == null) return null;
        var uiComp = go.GetComponent<UI>();
        if (uiComp == null)
        {
            Despawn(go);
            return null;
        }
        ui?.AttachToCore(realm, go);
        return uiComp;
    }

    public UI SpawnUIToSystem(string key)
    {
        var go = Spawn(key);
        if (go == null) return null;
        var uiComp = go.GetComponent<UI>();
        if (uiComp == null)
        {
            Despawn(go);
            return null;
        }
        ui?.AttachToSystem(go);
        return uiComp;
    }

    public UI SpawnPanel(string key, Realm realm = null, int extraOffset = 0)
    {
        var go = Spawn(key);
        if (go == null) return null;
        var uiComp = go.GetComponent<UI>();
        if (uiComp == null)
        {
            Despawn(go);
            return null;
        }
        ui?.OpenPanel(realm, go, extraOffset);
        return uiComp;
    }

    public UI SpawnModal(string key, Realm realm = null, int extraOffset = 0)
    {
        var go = Spawn(key);
        if (go == null) return null;
        var uiComp = go.GetComponent<UI>();
        if (uiComp == null)
        {
            Despawn(go);
            return null;
        }
        ui?.OpenModal(realm, go, extraOffset);
        return uiComp;
    }

    // Avatar spawn: instantiate prefab, bind WorldEntity, attach Service resolver, init/ready
    public Avatar SpawnAvatar(string key, WorldEntity entity, Realm realm = null, bool registerEntity = false)
    {
        var go = Spawn(key);
        if (go == null) return null;

        var avatar = go.GetComponent<Avatar>();
        if (avatar == null)
        {
            // Not an Avatar prefab; destroy or return to pool
            Despawn(go);
            return null;
        }

        // Optionally register entity into realm
        if (registerEntity && realm != null && entity != null)
        {
            var reg = realm.GetAbility<SpawnEntityAbility>();
            if (reg == null)
            {
                reg = new SpawnEntityAbility();
                realm.AddAbility(reg);
            }
            reg.AddWorldEntity(entity);
        }

        // Attach resolver, bind entity, lifecycle
        avatar.AttachService(Services);
        if (entity != null)
        {
            avatar.BindEntity(entity);
        }
        avatar.Initialize();
        avatar.Ready();
        return avatar;
    }

    public void Despawn(GameObject instance)
    {
        if (instance == null) return;
        if (pool != null)
        {
            pool.Despawn(instance);
        }
        else
        {
            GameObject.Destroy(instance);
        }
    }
}
