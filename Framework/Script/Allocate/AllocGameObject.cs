using System;
using System.Collections.Generic;
using UnityEngine;

public class AllocGameObject : MonoBehaviour
{
    public IReadOnlyList<GameObject> AllocatedObjects => allocatedObjects;
    
    [SerializeField] Transform parent;
    [SerializeField] GameObject prefab;

    List<GameObject> allocatedObjects = new(); 
    AbilitySet rootAbilitySet;
    ObjectPoolAbility objectPoolAbility;
    Transform selectedParent;

    private void Awake()
    {
        prefab.SetActive(false);
        selectedParent = prefab == null ? transform : parent;
    }

    public void OnEnable()
    {
        rootAbilitySet = Entry.RootRealm.AbilitySet;
        objectPoolAbility = rootAbilitySet.GetAbility<ObjectPoolAbility>();
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
        var allocateObject = objectPoolAbility.AllocateGameObject(prefab, selectedParent);
        allocateObject.SetActive(true);
        
        allocatedObjects.Add(allocateObject);
        
        return allocateObject;
    }

    public void DeallocateObjects()
    {
        foreach (var allocatedObject in allocatedObjects)
        {
            objectPoolAbility.DeallocateGameObject(allocatedObject);
        }
        
        allocatedObjects.Clear();
    }

    public bool TryDeallocateObject(int index)
    {
        if (allocatedObjects.Count <= index)
        {
            return false;
        }
        
        objectPoolAbility.DeallocateGameObject(allocatedObjects[index]);
        allocatedObjects.RemoveAt(index);

        return true;
    }
}
