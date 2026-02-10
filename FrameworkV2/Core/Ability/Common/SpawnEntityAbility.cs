using System;
using System.Collections.Generic;
using UnityEngine;

// Realm 하위 Entity 목록을 관리하는 Ability
public sealed class SpawnEntityAbility : Ability
{
    public event Action<Entity> EntityAdded;
    public event Action<Entity> EntityRemoved;

    public IReadOnlyList<Entity> Entities => entities;

    readonly List<Entity> entities = new();
    readonly Dictionary<string, Entity> entitiesById = new();
    Realm ownerRealm;

    // Ability 초기화 시 현재 Realm의 Entity를 스캔합니다.
    protected override void OnInitialize()
    {
        ownerRealm = GetComponent<Realm>();
        RefreshEntities();
    }

    // Realm 하위 Entity 목록을 갱신합니다.
    public void RefreshEntities()
    {
        if (ownerRealm == null)
        {
            ownerRealm = GetComponent<Realm>();
        }
        if (ownerRealm == null)
        {
            return;
        }

        var next = new List<Entity>();
        var root = ownerRealm.transform;
        for (int i = 0; i < root.childCount; i++)
        {
            var child = root.GetChild(i);
            var entity = child.GetComponent<Entity>();
            if (entity != null)
            {
                next.Add(entity);
            }
        }

        SyncEntities(next);
    }

    // Id로 Entity를 조회합니다.
    public bool TryGetById(string id, out Entity entity)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            entity = null;
            return false;
        }

        return entitiesById.TryGetValue(id, out entity);
    }

    // 별칭으로 Entity를 찾습니다.
    public IEnumerable<Entity> FindByAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            yield break;
        }

        for (int i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            if (entity != null && entity.HasAlias(alias))
            {
                yield return entity;
            }
        }
    }

    // 현재 목록과 새 목록을 비교해 추가/제거를 반영합니다.
    void SyncEntities(List<Entity> next)
    {
        var nextSet = new HashSet<Entity>(next);

        for (int i = entities.Count - 1; i >= 0; i--)
        {
            var entity = entities[i];
            if (entity == null || !nextSet.Contains(entity))
            {
                RemoveEntity(entity);
            }
        }

        for (int i = 0; i < next.Count; i++)
        {
            var entity = next[i];
            if (entity == null || entities.Contains(entity))
            {
                continue;
            }

            AddEntity(entity);
        }
    }

    // Entity를 목록에 추가합니다.
    void AddEntity(Entity entity)
    {
        if (entity == null)
        {
            return;
        }

        if (entitiesById.ContainsKey(entity.Id))
        {
            return;
        }

        entities.Add(entity);
        entitiesById.Add(entity.Id, entity);
        entity.AttachUpstreamResolver(AbilityResolver);
        entity.Initialize();
        entity.Ready();
        try { EntityAdded?.Invoke(entity); } catch { }
    }

    // Entity를 목록에서 제거합니다.
    void RemoveEntity(Entity entity)
    {
        if (entity == null)
        {
            return;
        }

        if (!entities.Remove(entity))
        {
            return;
        }

        entitiesById.Remove(entity.Id);
        entity.Uninitialize();
        entity.DetachUpstreamResolver();
        try { EntityRemoved?.Invoke(entity); } catch { }
    }
}
