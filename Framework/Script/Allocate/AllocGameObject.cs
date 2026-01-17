using System;
using System.Collections.Generic;
using UnityEngine;

public class AllocGameObject : MonoBehaviour
{
    public IReadOnlyList<GameObject> AllocatedObjects => allocatedObjects;
    
    [SerializeField] Transform parent;
    [SerializeField] GameObject prefab;

    List<GameObject> allocatedObjects = new(); 
    Environment env;
    ObjectPoolModule objectPoolModule;

    private void Awake()
    {
        prefab.SetActive(false);
    }

    public void OnEnable()
    {
        env = Universe.MainUniverse.Environment;
        objectPoolModule = env.GetModule<ObjectPoolModule>();
    }

    public void AllocateObject(int count)
    {
        AllocateObject(count, prefab);
    }

    public void AllocateObject(int count, GameObject prefab)
    {
        for (var i = 0; i < count; i++)
        {
            AllocateObject(prefab);
        }
    }

    public GameObject AllocateObject()
    {
        return AllocateObject(prefab);
    }

    public GameObject AllocateObject(GameObject prefab)
    {
        var allocateObject = objectPoolModule.AllocateGameObject(prefab, parent);
        allocateObject.SetActive(true);
        
        allocatedObjects.Add(allocateObject);
        
        return allocateObject;
    }

    public void DeallocateObjects()
    {
        foreach (var allocatedObject in allocatedObjects)
        {
            objectPoolModule.DeallocateGameObject(allocatedObject);
        }
        
        allocatedObjects.Clear();
    }

    public bool TryDeallocateObject(int index)
    {
        if (allocatedObjects.Count <= index)
        {
            return false;
        }
        
        objectPoolModule.DeallocateGameObject(allocatedObjects[index]);
        allocatedObjects.RemoveAt(index);

        return true;
    }
}
