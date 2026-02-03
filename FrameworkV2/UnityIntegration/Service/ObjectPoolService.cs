using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolService : Service
{
    readonly Dictionary<GameObject, Queue<GameObject>> prefabToPool = new();
    readonly Dictionary<GameObject, GameObject> instanceToPrefab = new();

    public GameObject Spawn(GameObject prefab)
    {
        if (prefab == null)
        {
            return null;
        }

        if (!prefabToPool.TryGetValue(prefab, out var pool))
        {
            pool = new();
            prefabToPool[prefab] = pool;
        }

        GameObject instance;
        if (pool.Count > 0)
        {
            instance = pool.Dequeue();
            if (instance == null)
            {
                // 방어: 파괴된 참조가 섞여있을 수 있음
                return Spawn(prefab);
            }
            instance.SetActive(true);
        }
        else
        {
            instance = GameObject.Instantiate(prefab);
            instanceToPrefab[instance] = prefab;
        }

        return instance;
    }

    public void Despawn(GameObject instance)
    {
        if (instance == null)
        {
            return;
        }

        if (!instanceToPrefab.TryGetValue(instance, out var prefab))
        {
            // 등록되지 않은 인스턴스는 안전하게 파괴 처리
            GameObject.Destroy(instance);
            return;
        }

        if (!prefabToPool.TryGetValue(prefab, out var pool))
        {
            pool = new();
            prefabToPool[prefab] = pool;
        }

        instance.SetActive(false);
        pool.Enqueue(instance);
    }

    protected override void OnUninitialize()
    {
        // 풀 비우기(간단 정리)
        foreach (var kv in prefabToPool)
        {
            var queue = kv.Value;
            while (queue.Count > 0)
            {
                var inst = queue.Dequeue();
                if (inst != null)
                {
                    GameObject.Destroy(inst);
                }
            }
        }
        prefabToPool.Clear();
        instanceToPrefab.Clear();
    }
}
