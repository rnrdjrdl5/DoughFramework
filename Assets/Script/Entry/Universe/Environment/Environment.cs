using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Environment
{
    List<Module> modules = new();

    public static Environment Create(GameObject universeObject, string[] modules)
    {
        Environment environment = new();
        environment.Initialize();
        
        foreach (var module in modules)
        {
            var createdModule = Module.Create(universeObject, environment, module);
            environment.modules.Add(createdModule);
        }

        foreach (var module in environment.modules)
        {
            module.Ready();
        }

        return environment;
    }
    
    public void Ready()
    {
        foreach (var module in modules)
        {
            module.Ready();
        }
    }
    
    protected virtual void Initialize()
    {
        
    }

    public ModuleType GetModule<ModuleType>() where ModuleType : Module
    {
        return modules.Where(trait => typeof(ModuleType).IsAssignableFrom(trait.GetType()))
            .Cast<ModuleType>()
            .FirstOrDefault();
    }
}
