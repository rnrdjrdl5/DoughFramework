using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Entity : MonoBehaviour
{
    static int nextUniqueId = 0;
    static int DefaultHierarchyLevel = 0;
    
    public Entity Parent => parent;
    public IReadOnlyList<IEntityData> EntityDatas => entityDatas;
    public int UniqueId => uniqueId;
    public AbilitySet AbilitySet => abilitySet;
    public AbilitySet RootAbilitySet => rootAbilitySet;

    AbilitySet rootAbilitySet;
    AbilitySet abilitySet = new();
    EntityRegistry children = new();
    List<IEntityData> entityDatas = new();
    
    Entity parent;
    int uniqueId;

    public void Initialize(AbilitySet abilitySet, Parameter parameter = null)
    {
        InitializeRootAbilitySet(abilitySet);
        Initialize(parameter);
    }

    public void InitializeRootAbilitySet(AbilitySet rootAbilitySet)
    {
        this.rootAbilitySet = rootAbilitySet;
    }
    
    public virtual void Initialize(Parameter parameter)
    {
        NextUniqueId();

        InitEntityDatas(parameter);
        InitAbilities(parameter);
    }

    void NextUniqueId()
    {
        uniqueId = ++nextUniqueId;
    }

    void InitEntityDatas(Parameter parameter)
    {
        entityDatas.Clear();
        
        var innerTypes = GetType()
            .GetCustomAttributes(typeof(EntityDataAttribute), true)
            .Cast<EntityDataAttribute>()
            .Select(attr => attr.Type);
        
        foreach (var type in innerTypes)
        {
            var entityData = System.Activator.CreateInstance(type) as IEntityData;
            entityDatas.Add(entityData);
            entityData.Initialize(parameter);
        }
    }

    void InitAbilities(Parameter parameter)
    {
        abilitySet.Abilities.Clear();

        var components = GetComponents<Ability>();
        if (components.Length > 0)
        {
            abilitySet.Abilities.AddRange(components);
        }

        foreach (var ability in abilitySet.Abilities)
        {
            ability.SetEntity(this);
            ability.Initialize(parameter);
        }
        foreach (var ability in abilitySet.Abilities)
        {
            ability.Ready();
        }
    }

    public virtual void Uninitialize()
    {
        UninitAbilities();
        UninitEntityDatas();
        
        RemoveChildren();
    }

    void UninitAbilities()
    {
        foreach (var ability in abilitySet.Abilities)
        {
            ability.Uninitialize();
        }
    }

    void UninitEntityDatas()
    {
        foreach (var entityData in entityDatas)
        {
            entityData.Uninitialize();
        }
    }

    public AbilityType GetAbility<AbilityType>() where AbilityType : Ability
    {
        return abilitySet.GetAbility<AbilityType>();
    }

    public EntityType AddEntity<EntityType>(string prefabPath, Parameter parameter = null) where EntityType : Entity, new()
    {
        var objectPoolModule = rootAbilitySet.GetAbility<ObjectPoolAbility>(); 
        
        var entityPrefab = Realm.LoadResources<GameObject>(prefabPath);
        var entityObject = objectPoolModule.AllocateGameObject(entityPrefab);
        var entity = entityObject.GetComponent<EntityType>();
        
        AddChild(entity);
        
        entity.Initialize(rootAbilitySet, parameter);
        
        return entity;
    }

    public void AddChild(Entity entity)
    {
        entity.parent = this;
        
        children.AddEntity(entity);
    }

    public IEnumerable<EntityType> GetChildren<EntityType>() where EntityType : Entity
    {
        return children.GetEntity<EntityType>();
    }

    public Entity GetChild(int uniqueId) => children.GetEntity(uniqueId);

    public void RemoveChild(Entity entity)
    {
        entity.Uninitialize();
        children.RemoveEntity(entity);
        
        var objectPoolModule = rootAbilitySet.GetAbility<ObjectPoolAbility>();
        objectPoolModule.DeallocateGameObject(entity.gameObject);
    }

    public void RemoveChildren()
    {
        for (int i = children.Entities.Count - 1; i >= 0; i--)
        {
            RemoveChild(children.Entities[i]);
        }
    }
    
    public EntityType GetRootParent<EntityType>() where EntityType : Entity
    {
        var current = this;
        var lastValidEntity = this;

        while (current != null)
        {
            if (current is EntityType targetType)
            {
                return targetType;
            }
            
            lastValidEntity = current;
            current = current.parent;
        }
        
        return lastValidEntity as EntityType;
    }

    public Entity GetRootParent()
    {
        var current = this;
        while (current.parent != null)
        {
            current = current.parent;
        }
        
        return current;
    }
    
    public TEntityData GetEntityData<TEntityData>() where TEntityData : class, IEntityData
    {
        if (EntityDatas == null)
        {
            return null;
        }
        
        return EntityDatas.Where(entity => typeof(TEntityData).IsAssignableFrom(entity.GetType()))
            .Cast<TEntityData>()
            .FirstOrDefault();
    }
}
