using System.Collections.Generic;
using UnityEngine;

public abstract partial class Universe : MonoBehaviour, IEnvironment
{
    public static List<Universe> universes = new();
    public static Universe MainUniverse => universes[0] == null ?  null : universes[0]; 
    
    public Environment Environment { get; private set; }
    
    public static Universe Create<UniverseType>() where UniverseType : Universe, new()
    {
        var universeObject = new GameObject();
        universeObject.name = typeof(UniverseType).Name;
        DontDestroyOnLoad(universeObject);
        
        var universe = universeObject.AddComponent<UniverseType>();
        universes.Add(universe);
        
        var environment = Environment.Create(universeObject, universe.GetModule());
        universe.Environment = environment;
        
        universe.Initialize();
        
        return universe;
    }

    public virtual void Ready()
    {
        Environment.Ready();
    }
    
    protected virtual void Initialize() { }

    protected virtual string[] GetModule()
    {
        return null;
    }
}
