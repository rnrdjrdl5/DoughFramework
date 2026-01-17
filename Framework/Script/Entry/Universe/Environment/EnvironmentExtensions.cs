using UnityEngine;

public static class EnvironmentExtensions
{
    public static Environment GetEnvironmentInParent(this Actor actor)
    {
        var targetActor = actor;
        var environment = actor.Environment; 
        
        while (environment != null)
        {
            targetActor = targetActor.Parent;
            if (targetActor == null)
            {
                break;
            }
            
            environment = targetActor.Environment;
        }

        return environment;
    }

    public static Environment GetEnviormentInParent(this GameObject gameObject)
    {
        var targetTransform = gameObject.transform;
        var environment = gameObject.GetComponent<IEnvironment>();
        
        while (environment != null)
        {
            targetTransform = targetTransform.parent;
            if (targetTransform == null)
            {
                break;
            }
            
            environment = targetTransform.GetComponent<IEnvironment>();
        }

        return environment?.Environment;
    }
}