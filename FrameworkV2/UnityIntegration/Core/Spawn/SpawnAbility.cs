using UnityEngine;

// 프리팹 스폰 및 UI 배치를 담당하는 Ability
public class SpawnAbility : Ability, ISpawnAbility
{
    ObjectPoolAbility pool;
    IUIAbility ui;

    protected override void OnReady()
    {
        ui = AbilityResolver?.GetAbility<UIAbility>();
        pool = AbilityResolver?.GetAbility<ObjectPoolAbility>();
    }

    // 프리팹을 스폰하여 GameObject를 반환합니다.
    public GameObject Spawn(string key)
    {
        if (string.IsNullOrEmpty(key)) return null;
        var prefab = AddressableAccess.LoadAsset<GameObject>(key);
        if (prefab == null) return null;
        if (pool == null) return GameObject.Instantiate(prefab);
        return pool.Spawn(prefab);
    }

    // Core UI 루트에 부착된 UI GameObject를 반환합니다.
    public GameObject SpawnUIToCore(string key, Realm realm = null)
    {
        var go = Spawn(key);
        if (go == null) return null;
        ui?.AttachToCore(realm, go);
        return go;
    }

    // System UI 루트에 부착된 UI GameObject를 반환합니다.
    public GameObject SpawnUIToSystem(string key)
    {
        var go = Spawn(key);
        if (go == null) return null;
        ui?.AttachToSystem(go);
        return go;
    }

    // Panel UI를 스폰하고 패널 루트에 부착합니다.
    public GameObject SpawnPanel(string key, Realm realm = null, int extraOffset = 0)
    {
        var go = Spawn(key);
        if (go == null) return null;
        ui?.OpenPanel(realm, go, extraOffset);
        return go;
    }

    // Modal UI를 스폰하고 모달 루트에 부착합니다.
    public GameObject SpawnModal(string key, Realm realm = null, int extraOffset = 0)
    {
        var go = Spawn(key);
        if (go == null) return null;
        ui?.OpenModal(realm, go, extraOffset);
        return go;
    }

    // 스폰된 GameObject를 반환하거나 파괴합니다.
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
