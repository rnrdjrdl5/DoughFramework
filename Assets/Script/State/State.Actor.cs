using System.Collections.Generic;
using System.Linq;

public class ActorState : State<Actor>
{
    public static StateType Create<StateType>(Actor actor) where StateType : ActorState, new()
    {
        StateType stateType = new();
        stateType.SetOwner(actor);

        return stateType;
    }
}
