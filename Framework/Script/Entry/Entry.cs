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
        rootRealm = Realm.LoadResources<Realm>(rootRealmPath);
        rootRealm.Initialize(rootRealm.RootTraitSet);
    }

    protected virtual void Ready()
    {
        
    }
}
