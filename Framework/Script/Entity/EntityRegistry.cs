using System.Collections.Generic;
using System.Linq;

public class EntityRegistry
{
    public List<Entity> Entities => entities;
    public event System.Action<Entity> OnAddEntity;
    public event System.Action<Entity> OnRemoveEntity;
    
    List<Entity> entities = new();
    
    public void AddEntity(Entity entity)
    {
        if (entities.Contains(entity))
        {
            return;
        }
        
        entities.Add(entity);
        OnAddEntity?.Invoke(entity);
    }

    public bool ContainsEntity(Entity entity)
    {
        return entity != null && entities.Contains(entity);
    }

    public bool RemoveEntity(Entity entity)
    {
        if (!entities.Contains(entity))
        {
            return false;
        }
        
        entities.Remove(entity);
        OnRemoveEntity?.Invoke(entity);
        return true;
    }

    public IEnumerable<EntityType> GetEntities<EntityType>() where EntityType : Entity
    {
        return entities.Where(entity => typeof(EntityType).IsAssignableFrom(entity.GetType()))
            .Cast<EntityType>();
    }

    public Entity GetEntity(int uniqueId) => entities.First(entity => entity.UniqueId == uniqueId);

    public EntityType GetEntity<EntityType>() where EntityType : Entity
    {
        return entities.FirstOrDefault(entity => typeof(EntityType).IsAssignableFrom(entity.GetType())) as EntityType;
    }
}
