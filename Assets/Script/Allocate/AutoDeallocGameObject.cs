using System;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeallocGameObject : MonoBehaviour
{
    [SerializeField] float destroyTime;
    
    Environment env;
    ObjectPoolModule objectPoolModule;
    float duration;

    private void Awake()
    {
        env = Universe.MainUniverse.Environment;
        objectPoolModule = env.GetModule<ObjectPoolModule>();
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
        
        objectPoolModule.DeallocateGameObject(gameObject);
    }
}
