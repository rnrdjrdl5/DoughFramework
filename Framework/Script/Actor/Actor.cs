using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Actor : MonoBehaviour, IEnvironment
{
    static int nextUniqueId = 0;
    static int DefaultHierarchyLevel = 0;
    
    public Environment Environment { get; private set; }
    public Actor Parent => parent;
    public IReadOnlyList<IActorData> ActorDatas => actorDatas;
    public IReadOnlyList<Trait> Traits => traits;
    public int UniqueId => uniqueId;
    
    ActorRegistry children = new();
    List<Trait> traits = new();
    List<IActorData> actorDatas = new();
    
    Actor parent;
    int uniqueId;

    public void Initialize(Environment environment, Parameter parameter = null)
    {
        InitializeEnvironment(environment);
        Initialize(parameter);
    }

    public void InitializeEnvironment(Environment environment)
    {
        Environment = environment;
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
        traits.Clear();

        var components = GetComponents<Trait>();
        if (components.Length > 0)
        {
            traits.AddRange(components);
        }

        foreach (var trait in traits)
        {
            trait.SetActor(this);
            trait.Initialize(parameter);
        }
        foreach (var trait in traits)
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
        foreach (var trait in traits)
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
        return traits.Where(trait => typeof(TraitType).IsAssignableFrom(trait.GetType()))
            .Cast<TraitType>()
            .FirstOrDefault();
    }

    public ActorType AddActor<ActorType>(string prefabPath, Parameter parameter = null) where ActorType : Actor, new()
    {
        var objectPoolModule = Environment.GetModule<ObjectPoolModule>(); 
        
        var actorPrefab = Universe.LoadResources<GameObject>(prefabPath);
        var actorObject = objectPoolModule.AllocateGameObject(actorPrefab);
        var actor = actorObject.GetComponent<ActorType>();
        
        AddChild(actor);
        
        actor.Initialize(Environment, parameter);
        
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
        
        var objectPoolModule = Environment.GetModule<ObjectPoolModule>();
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
