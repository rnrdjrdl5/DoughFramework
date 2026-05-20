using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[EntityData(typeof(MessageBus))]
public partial class Entity : MonoBehaviour , IControlled, IUniqueId
{
    class OwnedEntityData
    {
        public IEntityData Data;
        public bool IsOwned;
    }

    static int DefaultHierarchyLevel = 0;
    
    public event Action<Entity> OnChildAdded;
    public event Action<Entity> OnChildRemoved;

    public Entity Parent => parent;
    public IReadOnlyList<IEntityData> EntityDatas => entityDatas;
    public long UniqueId { get; set; }
    public AbilitySet AbilitySet => abilitySet;
    public AbilitySet RootAbilitySet => rootAbilitySet;
    public MessageBus MessageBus => messageBus;
    public bool IsReady => isReady;
    
    readonly List<IEntityData> entityDatas = new();
    readonly List<OwnedEntityData> ownedEntityDatas = new();
    readonly List<OwnedEntityData> overrideEntityDatas = new();
    AbilitySet rootAbilitySet;
    AbilitySet abilitySet = new();
    EntityRegistry children = new();
    MessageBus messageBus;
    Entity parent;
    bool isReady;

    public void Initialize(AbilitySet abilitySet, IInitData initData = null)
    {
        PreInitialize(initData);
        
        InitializeRootAbilitySet(abilitySet);
        Initialize(initData);
    }

    public void InitializeRootAbilitySet(AbilitySet rootAbilitySet)
    {
        this.rootAbilitySet = rootAbilitySet;
    }

    protected virtual void PreInitialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        
        if (initData is IUniqueId uniqueId)
        {
            SetUniqueId(uniqueId.UniqueId);
        }

        if (initData is IPositionData positionData)
        {
            transform.position = positionData.Position;
        }
    }
    
    protected virtual void Initialize(IInitData initData = null)
    {
        InitEntityDatas(initData);
        InitMessageBus();
        InitAbilities(initData);
    }

    public virtual void Ready()
    {
        foreach (var ability in abilitySet.Abilities)
        {
            ability.Ready();
        }

        isReady = true;
    }

    void SetUniqueId(long uid = 0)
    {
        UniqueId = uid == 0 ? IDLogic.NewUniqueId() : uid;
    }
    
    void InitEntityDatas(IInitData initData)
    {
        ownedEntityDatas.Clear();
        entityDatas.Clear();
        
        var innerTypes = GetType()
            .GetCustomAttributes(typeof(EntityDataAttribute), true)
            .Cast<EntityDataAttribute>()
            .Select(attr => attr.Type);
        
        foreach (var type in innerTypes)
        {
            var overrideEntry = overrideEntityDatas.FirstOrDefault(entry => entry.Data.GetType() == type);
            if (overrideEntry != null)
            {
                AddOwnedEntityData(overrideEntry);
                continue;
            }
            
            var entityData = System.Activator.CreateInstance(type) as IEntityData;
            AddOwnedEntityData(new OwnedEntityData
            {
                Data = entityData,
                IsOwned = true
            });
            entityData.Initialize(initData);
        }
    }

    void AddOwnedEntityData(OwnedEntityData ownedEntityData)
    {
        if (ownedEntityData?.Data == null)
        {
            return;
        }

        ownedEntityDatas.Add(ownedEntityData);
        entityDatas.Add(ownedEntityData.Data);
    }

    void InitMessageBus()
    {
        messageBus = GetEntityData<MessageBus>();
        if (messageBus == null)
        {
            return;
        }

        foreach (var entityData in entityDatas)
        {
            if (entityData is IMessageBus messageBusOwner)
            {
                messageBusOwner.MessageBus = messageBus;
                messageBusOwner.OnSetMessageBus();
            }
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
        foreach (var ownedEntityData in ownedEntityDatas)
        {
            if (ownedEntityData.IsOwned)
            {
                ownedEntityData.Data.Uninitialize();
            }
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
        OnChildAdded?.Invoke(entity);
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
        TryRemoveChild(entity);
    }

    public bool TryRemoveChild(Entity entity)
    {
        if (!children.ContainsEntity(entity))
        {
            return false;
        }

        OnChildRemoved?.Invoke(entity);
        entity.Uninitialize();
        children.RemoveEntity(entity);
        
        var objectPoolModule = rootAbilitySet.GetAbility<ObjectPoolAbility>();
        objectPoolModule.DeallocateGameObject(entity.gameObject);

        return true;
    }

    public void RemoveChildren()
    {
        for (int i = children.Entities.Count - 1; i >= 0; i--)
        {
            RemoveChild(children.Entities[i]);
        }
    }
    
    public EntityType GetParent<EntityType>() where EntityType : Entity
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

    public EntityType GetFromRoot<EntityType>() where EntityType : Entity
    {
        var rootParent = GetRootParent();
        return rootParent?.GetInChildrenRecursive<EntityType>();
    }

    public EntityType GetInChildrenRecursive<EntityType>() where EntityType : Entity
    {
        if (this is EntityType self)
        {
            return self;
        }

        return GetInChildrenRecursive<EntityType>(this);
    }

    static EntityType GetInChildrenRecursive<EntityType>(Entity entity) where EntityType : Entity
    {
        if (entity == null)
        {
            return null;
        }

        for (var i = 0; i < entity.children.Entities.Count; i++)
        {
            var child = entity.children.Entities[i];
            if (child is EntityType target)
            {
                return target;
            }

            if (GetInChildrenRecursive<EntityType>(child) is EntityType nestedTarget)
            {
                return nestedTarget;
            }
        }

        return null;
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

    public void AddOverrideEntityData(IEntityData entityData, bool isOwned = false)
    {
        if (entityData == null)
        {
            return;
        }

        overrideEntityDatas.Add(new OwnedEntityData
        {
            Data = entityData,
            IsOwned = isOwned
        });
    }
}
