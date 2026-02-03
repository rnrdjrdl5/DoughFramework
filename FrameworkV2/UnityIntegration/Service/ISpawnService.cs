using UnityEngine;

public interface ISpawnService : IService
{
    // Generic GameObject spawn by Addressables key
    GameObject Spawn(string key);

    // UI spawns (typed)
    UI SpawnUIToCore(string key, Realm realm = null);
    UI SpawnUIToSystem(string key);
    UI SpawnPanel(string key, Realm realm = null, int extraOffset = 0);
    UI SpawnModal(string key, Realm realm = null, int extraOffset = 0);

    // Avatar spawn: instantiate prefab, bind entity, attach resolver, initialize/ready
    Avatar SpawnAvatar(string key, WorldEntity entity, Realm realm = null, bool registerEntity = false);

    // Return instance to pool or destroy if unmanaged
    void Despawn(GameObject instance);
}
