using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Actor : MonoBehaviour
{
    static int nextUniqueId = 0;
    static int DefaultHierarchyLevel = 0;
    
    public Actor Parent => parent;
    public IReadOnlyList<IActorData> ActorDatas => actorDatas;
    public int UniqueId => uniqueId;
    public AbilitySet AbilitySet => abilitySet;
    public AbilitySet RootAbilitySet => rootAbilitySet;

    AbilitySet rootAbilitySet;
    AbilitySet abilitySet = new();
    ActorRegistry children = new();
    List<IActorData> actorDatas = new();
    
    Actor parent;
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

        InitActorDatas(parameter);
        InitAbilities(parameter);
    }

    void NextUniqueId()
    {
        uniqueId = ++nextUniqueId;
    }

    void InitActorDatas(Parameter parameter)
    {
        actorDatas.Clear();
        
        var innerTypes = GetType()
            .GetCustomAttributes(typeof(ActorDataAttribute), true)
            .Cast<ActorDataAttribute>()
            .Select(attr => attr.Type);
        
        foreach (var type in innerTypes)
        {
            var actorData = System.Activator.CreateInstance(type) as IActorData;
            actorDatas.Add(actorData);
            actorData.Initialize(parameter);
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
            ability.SetActor(this);
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
        UninitActorDatas();
        
        RemoveChildren();
    }

    void UninitAbilities()
    {
        foreach (var ability in abilitySet.Abilities)
        {
            ability.Uninitialize();
        }
    }

    void UninitActorDatas()
    {
        foreach (var actorData in actorDatas)
        {
            actorData.Uninitialize();
        }
    }

    public AbilityType GetAbility<AbilityType>() where AbilityType : Ability
    {
        return abilitySet.GetAbility<AbilityType>();
    }

    public ActorType AddActor<ActorType>(string prefabPath, Parameter parameter = null) where ActorType : Actor, new()
    {
        var objectPoolModule = rootAbilitySet.GetAbility<ObjectPoolAbility>(); 
        
        var actorPrefab = Realm.LoadResources<GameObject>(prefabPath);
        var actorObject = objectPoolModule.AllocateGameObject(actorPrefab);
        var actor = actorObject.GetComponent<ActorType>();
        
        AddChild(actor);
        
        actor.Initialize(rootAbilitySet, parameter);
        
        return actor;
    }

    public void AddChild(Actor actor)
    {
        actor.parent = this;
        
        children.AddActor(actor);
    }

    public IEnumerable<ActorType> GetChildren<ActorType>() where ActorType : Actor
    {
        return children.GetActor<ActorType>();
    }

    public Actor GetChild(int uniqueId) => children.GetActor(uniqueId);

    public void RemoveChild(Actor actor)
    {
        actor.Uninitialize();
        children.RemoveActor(actor);
        
        var objectPoolModule = rootAbilitySet.GetAbility<ObjectPoolAbility>();
        objectPoolModule.DeallocateGameObject(actor.gameObject);
    }

    public void RemoveChildren()
    {
        for (int i = children.Actors.Count - 1; i >= 0; i--)
        {
            RemoveChild(children.Actors[i]);
        }
    }
    
    public ActorType GetRootParent<ActorType>() where ActorType : Actor
    {
        var current = this;
        var lastValidActor = this;

        while (current != null)
        {
            if (current is ActorType targetType)
            {
                return targetType;
            }
            
            lastValidActor = current;
            current = current.parent;
        }
        
        return lastValidActor as ActorType;
    }

    public Actor GetRootParent()
    {
        var current = this;
        while (current.parent != null)
        {
            current = current.parent;
        }
        
        return current;
    }
    
    public TActorData GetActorData<TActorData>() where TActorData : class, IActorData
    {
        if (ActorDatas == null)
        {
            return null;
        }
        
        return ActorDatas.Where(actor => typeof(TActorData).IsAssignableFrom(actor.GetType()))
            .Cast<TActorData>()
            .FirstOrDefault();
    }
}
