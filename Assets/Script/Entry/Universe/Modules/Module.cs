using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Module
{
    // NOTE : Environment를 직접 가지고 있어서는 안 된다. 좋은 방법을 생각 해 보자.
    public Environment Environment => environment;
    
    Environment environment;
    
    public virtual void Ready() { }

    protected virtual void Initialize(GameObject universeObject, Environment environment)
    {
        this.environment = environment;
    }
    
    public static Module Create(GameObject universeObject, Environment environment, string module)
    {
        var createdModule = System.Activator.CreateInstance(System.Type.GetType(module)) as Module;
        createdModule.Initialize(universeObject, environment);

        return createdModule;
    }
}
