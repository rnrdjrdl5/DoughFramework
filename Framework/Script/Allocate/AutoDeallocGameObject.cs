using System;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeallocGameObject : MonoBehaviour
{
    [SerializeField] float destroyTime;
    
    AbilitySet abilitySet;
    ObjectPoolAbility objectPoolAbility;
    float duration;

    private void Awake()
    {
        abilitySet = Entry.RootRealm.RootAbilitySet;
        objectPoolAbility = abilitySet.GetAbility<ObjectPoolAbility>();
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
        
        objectPoolAbility.DeallocateGameObject(gameObject);
    }
}
