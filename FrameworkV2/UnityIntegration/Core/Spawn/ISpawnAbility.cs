using UnityEngine;

// 프리팹 스폰 Ability 인터페이스
public interface ISpawnAbility
{
    GameObject SpawnUIToCore(string key, Realm realm = null);
    GameObject SpawnUIToSystem(string key);
    GameObject SpawnPanel(string key, Realm realm = null, int extraOffset = 0);
    GameObject SpawnModal(string key, Realm realm = null, int extraOffset = 0);
}
