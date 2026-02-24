using System;
using System.Collections.Generic;
using UnityEngine;

// Application 시작구간 , 첫 초기화 시점
public abstract class Entry : MonoBehaviour
{
    public static Realm RootRealm => rootRealm;
    
    static Realm rootRealm;
    [SerializeField] string rootRealmPath;
    
    void Awake()
    {
        Initialize();
        Ready();
    }

    protected virtual void Initialize()
    {
        var realmPrefab = Realm.LoadResources<GameObject>(rootRealmPath);
        var realmObject = Instantiate(realmPrefab);
        rootRealm = realmObject.GetComponent<Realm>();
        rootRealm.Initialize(rootRealm.TraitSet);
    }

    protected virtual void Ready()
    {
        
    }
}
