using System;
using System.Collections.Generic;

public sealed class SpawnEntityAbility : Ability
{
    public event Action<WorldEntity> WorldEntityAdded;
    public event Action<WorldEntity> WorldEntityRemoved;

    public IReadOnlyList<LocalEntity> LocalEntities => localEntities;
    public IReadOnlyList<WorldEntity> WorldEntities => worldEntities;

    readonly List<LocalEntity> localEntities = new();
    readonly List<WorldEntity> worldEntities = new();
    readonly Dictionary<string, Entity> entitiesById = new();

    public bool AddLocalEntity(LocalEntity entity)
    {
        if (!TryRegisterEntity(entity))
        {
            return false;
        }

        entity.AttachUpstreamResolver(AbilityResolver);
        localEntities.Add(entity);
        entity.Initialize();
        entity.Ready();
        return true;
    }

    public bool AddWorldEntity(WorldEntity entity)
    {
        if (!TryRegisterEntity(entity))
        {
            return false;
        }

        entity.AttachUpstreamResolver(AbilityResolver);
        worldEntities.Add(entity);
        entity.Initialize();
        entity.Ready();
        try { WorldEntityAdded?.Invoke(entity); } catch { }
        return true;
    }

    public bool RemoveLocalEntity(LocalEntity entity)
    {
        if (entity == null || !localEntities.Remove(entity))
        {
            return false;
        }

        entitiesById.Remove(entity.Id);
        entity.Uninitialize();
        entity.DetachUpstreamResolver();
        return true;
    }

    public bool RemoveWorldEntity(WorldEntity entity)
    {
        if (entity == null || !worldEntities.Remove(entity))
        {
            return false;
        }

        entitiesById.Remove(entity.Id);
        entity.Uninitialize();
        entity.DetachUpstreamResolver();
        try { WorldEntityRemoved?.Invoke(entity); } catch { }
        return true;
    }

    public bool TryGetById(string id, out Entity entity)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            entity = null;
            return false;
        }

        return entitiesById.TryGetValue(id, out entity);
    }

    public IEnumerable<Entity> FindByAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            yield break;
        }

        foreach (var e in localEntities)
        {
            if (e.HasAlias(alias))
            {
                yield return e;
            }
        }

        foreach (var e in worldEntities)
        {
            if (e.HasAlias(alias))
            {
                yield return e;
            }
        }
    }
    
    bool TryRegisterEntity(Entity entity)
    {
        if (entity == null)
        {
            return false;
        }

        if (entitiesById.ContainsKey(entity.Id))
        {
            return false;
        }

        entitiesById.Add(entity.Id, entity);
        return true;
    }
}
