using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolAbility : Ability
{
    Dictionary<GameObject, List<GameObject>> prefabToDeallocObjects = new();
    Dictionary<GameObject, List<GameObject>> prefabToAllocObjects = new();
    Dictionary<GameObject, GameObject> allocObjectToPrefab = new();
    GameObject rootDeallocObject;
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);
        
        rootDeallocObject = new();
        rootDeallocObject.name = "DellocObject";
        rootDeallocObject.transform.parent = Actor.transform;
        rootDeallocObject.SetActive(false);
    }

    public GameObject AllocateGameObject(GameObject prefab, Transform parent = null)
    {
        GameObject result = null;

        if (!prefabToDeallocObjects.TryGetValue(prefab, out var deallocObjects))
        {
            deallocObjects = new();
            
            prefabToDeallocObjects.Add(prefab, deallocObjects);
        }

        if (deallocObjects.Count == 0)
        {
            result = GameObject.Instantiate(prefab);
            result.name = result.name.Replace("(Clone)", "");
        }
        else
        {
            result = deallocObjects[0];
            
            deallocObjects.Remove(result);
        }

        if (!prefabToAllocObjects.TryGetValue(prefab, out var allocObjects))
        {
            allocObjects = new();
            
            prefabToAllocObjects.Add(prefab, allocObjects);
        }
        
        allocObjects.Add(result);
        
        allocObjectToPrefab.TryAdd(result, prefab);
        
        result.transform.parent = parent;
        if (parent ==null)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.MoveGameObjectToScene(result, activeScene);
        }
        
        result.transform.localPosition = Vector3.zero;
        result.transform.localScale = Vector3.one;
        

        return result;
    }

    public void DeallocateGameObject(GameObject targetObject)
    {
        allocObjectToPrefab.Remove(targetObject, out var prefab);

        if (prefabToDeallocObjects.TryGetValue(prefab, out var deallocObjects))
        {
            deallocObjects.Add(targetObject);
        }
        
        if (prefabToAllocObjects.TryGetValue(prefab, out var allocObjects))
        {
            targetObject.transform.parent = rootDeallocObject.transform;
            allocObjects.Remove(targetObject);
        }
    }
}