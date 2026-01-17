using System.Collections.Generic;
using System.Linq;

public class ActorRegistry
{
    public List<Actor> Actors => actors;
    public event System.Action<Actor> OnAddActor;
    public event System.Action<Actor> OnRemoveActor;
    
    List<Actor> actors = new();
    
    public void AddActor(Actor actor)
    {
        if (actors.Contains(actor))
        {
            return;
        }
        
        actors.Add(actor);
        OnAddActor?.Invoke(actor);
    }

    public void RemoveActor(Actor actor)
    {
        if (!actors.Contains(actor))
        {
            return;
        }
        
        actors.Remove(actor);
        OnRemoveActor?.Invoke(actor);
    }

    public IEnumerable<ActorType> GetActor<ActorType>() where ActorType : Actor
    {
        return actors.Where(actor => typeof(ActorType).IsAssignableFrom(actor.GetType()))
            .Cast<ActorType>();
    }

    public Actor GetActor(int uniqueId) => actors.First(actor => actor.UniqueId == uniqueId);
}