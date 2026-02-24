using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// 미리 스테이지를 만들어서 캐싱해둘 수 있다.
// State 형태로 스테이지를 관리한다. 
// 스테이지 전환 시 전환 Function 호출

public class RealmTrait : Trait
{
    List<Realm> realms = new();
    ObjectPoolTrait objectPoolTrait;

    public override void Ready()
    {
        base.Ready();
        
        objectPoolTrait = Actor.RootTraitSet.GetTrait<ObjectPoolTrait>();
    }

    public RealmType GetRealm<RealmType>() where RealmType : Realm
    {
        return realms.Where(trait => typeof(RealmType).IsAssignableFrom(trait.GetType()))
            .Cast<RealmType>()
            .FirstOrDefault();
    }

    public RealmType AddRealm<RealmType>(string prefabPath) where RealmType : Realm, new ()
    {
        var realmPrefab = Realm.LoadResources<GameObject>(prefabPath);
        var realmObject = objectPoolTrait.AllocateGameObject(realmPrefab);
        
        var realm = realmObject.GetComponent<RealmType>();
        realm.Initialize(Actor.RootTraitSet);
        
        realms.Add(realm);
        
        return realm;
    }

    public void RemoveRealm(Realm realm)
    {
        realm.Uninitialize();
        realms.Remove(realm);
        
        objectPoolTrait.DeallocateGameObject(realm.gameObject);
    }
}