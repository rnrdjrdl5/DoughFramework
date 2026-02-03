using System.Collections.Generic;
using UnityEngine;

// Realm 1개당 1 Presence: 아바타/UI/동적 Realm 생성 오케스트레이션
public class RealmPresence : MonoBehaviour
{
    [SerializeField] Transform avatarsRoot;
    [SerializeField] Transform uiRoot;

    static readonly Dictionary<string, RealmPresence> registry = new();

    IServiceResolver services;
    Realm realm;
    ISpawnService spawner;
    IUIService uiService;
    BuildRealmAbility buildAbility;

    public Realm Realm => realm;
    public Transform AvatarsRoot => avatarsRoot;
    public Transform UIRoot => uiRoot;

    // 정적: 존재 보장 생성자
    public static RealmPresence GetOrCreate(Realm realm, IServiceResolver services)
    {
        if (realm == null) return null;
        if (registry.TryGetValue(realm.Id, out var existing) && existing != null)
        {
            return existing;
        }

        var go = new GameObject($"RealmPresence_{realm.Id}");
        var rp = go.AddComponent<RealmPresence>();
        rp.AttachService(services);
        rp.Bind(realm);
        rp.Initialize();
        rp.Ready();
        return rp;
    }

    public void AttachService(IServiceResolver resolver)
    {
        services = resolver;
    }

    public void Bind(Realm r)
    {
        realm = r;
    }

    public void Initialize()
    {
        if (realm == null) return;
        if (!registry.ContainsKey(realm.Id))
        {
            registry[realm.Id] = this;
        }

        ResolveServices();
        EnsureRoots();
        EnsureUIContextParented();
        SubscribeBuildEvents();
        SubscribeEntityEvents();
    }

    public void Ready()
    {
        // 현 단계에선 추가 작업 없음
    }

    public void Uninitialize()
    {
        UnsubscribeBuildEvents();
        UnsubscribeEntityEvents();

        if (realm != null)
        {
            if (registry.TryGetValue(realm.Id, out var self) && self == this)
            {
                registry.Remove(realm.Id);
            }
        }
    }

    void ResolveServices()
    {
        spawner = services?.Get<ISpawnService>();
        uiService = services?.Get<IUIService>();
    }

    void EnsureRoots()
    {
        if (avatarsRoot == null)
        {
            avatarsRoot = new GameObject("Avatars").transform;
            avatarsRoot.SetParent(transform, false);
        }
        if (uiRoot == null)
        {
            uiRoot = new GameObject("UI").transform;
            uiRoot.SetParent(transform, false);
        }
    }

    void EnsureUIContextParented()
    {
        if (uiService == null || realm == null) return;
        var core = uiService.GetCoreRoot(realm);
        if (core != null && core.parent != null)
        {
            var ctxRoot = core.parent;
            ctxRoot.SetParent(uiRoot, false);
        }
    }

    void SubscribeBuildEvents()
    {
        buildAbility = realm?.GetAbility<BuildRealmAbility>();
        if (buildAbility != null)
        {
            buildAbility.RealmBuilt += OnChildRealmBuilt;
        }
    }

    void UnsubscribeBuildEvents()
    {
        if (buildAbility != null)
        {
            buildAbility.RealmBuilt -= OnChildRealmBuilt;
            buildAbility = null;
        }
    }

    void OnChildRealmBuilt(Realm child)
    {
        if (child == null) return;
        GetOrCreate(child, services);
    }

    void SubscribeEntityEvents()
    {
        var reg = realm?.GetAbility<SpawnEntityAbility>();
        if (reg == null) return;
        reg.WorldEntityAdded += OnWorldEntityAdded;
        reg.WorldEntityRemoved += OnWorldEntityRemoved;
    }

    void UnsubscribeEntityEvents()
    {
        var reg = realm?.GetAbility<SpawnEntityAbility>();
        if (reg == null) return;
        reg.WorldEntityAdded -= OnWorldEntityAdded;
        reg.WorldEntityRemoved -= OnWorldEntityRemoved;
    }

    void OnWorldEntityAdded(WorldEntity e)
    {
        if (e == null || spawner == null) return;

        // 프리팹 키는 일단 고정 "Avatar"
        const string key = "Avatar";
        var avatar = spawner.SpawnAvatar(key, e, realm, registerEntity: false);
        if (avatar == null) return;
        var go = avatar.gameObject;
        if (go != null)
        {
            go.transform.SetParent(avatarsRoot != null ? avatarsRoot : transform, false);
        }
    }

    void OnWorldEntityRemoved(WorldEntity e)
    {
        if (e == null) return;
        var parent = avatarsRoot != null ? avatarsRoot : transform;
        var arr = parent.GetComponentsInChildren<Avatar>(true);
        for (int i = 0; i < arr.Length; i++)
        {
            var a = arr[i];
            if (a == null) continue;
            if (!ReferenceEquals(a.Entity, e)) continue;

            var go = a.gameObject;
            if (go == null) continue;

            if (spawner != null)
            {
                spawner.Despawn(go);
            }
            else
            {
                Destroy(go);
            }
        }
    }

}
