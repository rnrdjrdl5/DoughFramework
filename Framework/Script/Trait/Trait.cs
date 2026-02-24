using UnityEngine;

public abstract class Trait : MonoBehaviour, IData
{
    public Actor Actor => actor;
    
    Actor actor;

    public virtual void Initialize(Parameter parameter)
    {

    }

    public virtual void Uninitialize()
    {

    }

    public virtual void Ready()
    {
        
    }

    public void SetActor(Actor actor)
    {
        this.actor = actor;
    }
}
