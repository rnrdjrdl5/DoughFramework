using System;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeallocGameObject : MonoBehaviour
{
    [SerializeField] float destroyTime;
    
    TraitSet traitSet;
    ObjectPoolTrait objectPoolTrait;
    float duration;

    private void Awake()
    {
        traitSet = Entry.RootRealm.RootTraitSet;
        objectPoolTrait = traitSet.GetTrait<ObjectPoolTrait>();
    }

    public void OnEnable()
    {
        duration = 0.0f;
    }

    public void Update()
    {
        duration += Time.deltaTime;
        if (destroyTime > duration)
        {
            return;
        }
        
        objectPoolTrait.DeallocateGameObject(gameObject);
    }
}
