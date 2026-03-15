using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[EntityData(typeof(MessageBus))]
public partial class Entity : MonoBehaviour , IControlled
{
    static int nextUniqueId = 0;
    static int DefaultHierarchyLevel = 0;
    
    public Entity Parent => parent;
    public IReadOnlyList<IEntityData> EntityDatas => entityDatas;
    public int UniqueId => uniqueId;
    public AbilitySet AbilitySet => abilitySet;
    public AbilitySet RootAbilitySet => rootAbilitySet;
    public MessageBus MessageBus => messageBus;
    public bool IsReady => isReady;

    AbilitySet rootAbilitySet;
    AbilitySet abilitySet = new();
    EntityRegistry children = new();
    List<IEntityData> entityDatas = new();
    MessageBus messageBus;
    
    Entity parent;
    int uniqueId;

    bool isReady;

    public void Initialize(AbilitySet abilitySet, IInitData initData = null)
    {
        InitializeRootAbilitySet(abilitySet);
        Initialize(initData);
    }

    public void InitializeRootAbilitySet(AbilitySet rootAbilitySet)
    {
        this.rootAbilitySet = rootAbilitySet;
    }
    
    public virtual void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        NextUniqueId();

        InitEntityDatas(initData);
        InitAbilities(initData);

        messageBus = GetEntityData<MessageBus>();
    }

    public virtual void Ready()
    {
        foreach (var ability in abilitySet.Abilities)
        {
            ability.Ready();
        }

        isReady = true;
    }

    void NextUniqueId()
    {
        uniqueId = ++nextUniqueId;
    }

    void InitEntityDatas(IInitData initData)
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
            entityData.Initialize(initData);
        }
    }

    void InitAbilities(IInitData initData)
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
            ability.Initialize(initData);
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

    public EntityType AddEntity<EntityType>(string prefabPath, IInitData initData = null, Action<EntityType> OnCreate = null) where EntityType : Entity, new()
    {
        var objectPoolAbility = rootAbilitySet.GetAbility<ObjectPoolAbility>(); 
        
        var entityPrefab = Realm.LoadResources<GameObject>(prefabPath);
        var entityObject = objectPoolAbility.AllocateGameObject(entityPrefab);
        var entity = entityObject.GetComponent<EntityType>();
        
        AddChild(entity);
        OnCreate?.Invoke(entity);
        
        entity.Initialize(rootAbilitySet, initData);
        entity.Ready();
        
        return entity;
    }

    public void AddChild(Entity entity)
    {
        SetParent(entity);
        
        children.AddEntity(entity);
    }

    public void SetParent(Entity entity)
    {
        entity.parent = this;
    }

    public IEnumerable<EntityType> GetChildren<EntityType>() where EntityType : Entity
    {
        return children.GetEntities<EntityType>();
    }
    
    public EntityType GetChild<EntityType>() where EntityType : Entity
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
