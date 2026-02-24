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
    public TraitSet TraitSet => traitSet;
    public TraitSet RootTraitSet => rootTraitSet;

    TraitSet rootTraitSet;
    TraitSet traitSet = new();
    ActorRegistry children = new();
    List<IActorData> actorDatas = new();
    
    Actor parent;
    int uniqueId;

    public void Initialize(TraitSet traitSet, Parameter parameter = null)
    {
        InitializeRootTraitSet(traitSet);
        Initialize(parameter);
    }

    public void InitializeRootTraitSet(TraitSet rootTraitSet)
    {
        this.rootTraitSet = rootTraitSet;
    }
    
    public virtual void Initialize(Parameter parameter)
    {
        NextUniqueId();

        InitActorDatas(parameter);
        InitTraits(parameter);
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

    void InitTraits(Parameter parameter)
    {
        traitSet.Traits.Clear();

        var components = GetComponents<Trait>();
        if (components.Length > 0)
        {
            traitSet.Traits.AddRange(components);
        }

        foreach (var trait in traitSet.Traits)
        {
            trait.SetActor(this);
            trait.Initialize(parameter);
        }
        foreach (var trait in traitSet.Traits)
        {
            trait.Ready();
        }
    }

    public virtual void Uninitialize()
    {
        UninitTraits();
        UninitActorDatas();
        
        RemoveChildren();
    }

    void UninitTraits()
    {
        foreach (var trait in traitSet.Traits)
        {
            trait.Uninitialize();
        }
    }

    void UninitActorDatas()
    {
        foreach (var actorData in actorDatas)
        {
            actorData.Uninitialize();
        }
    }

    public TraitType GetTrait<TraitType>() where TraitType : Trait
    {
        return traitSet.GetTrait<TraitType>();
    }

    public ActorType AddActor<ActorType>(string prefabPath, Parameter parameter = null) where ActorType : Actor, new()
    {
        var objectPoolModule = rootTraitSet.GetTrait<ObjectPoolTrait>(); 
        
        var actorPrefab = Realm.LoadResources<GameObject>(prefabPath);
        var actorObject = objectPoolModule.AllocateGameObject(actorPrefab);
        var actor = actorObject.GetComponent<ActorType>();
        
        AddChild(actor);
        
        actor.Initialize(rootTraitSet, parameter);
        
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
        
        var objectPoolModule = rootTraitSet.GetTrait<ObjectPoolTrait>();
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
