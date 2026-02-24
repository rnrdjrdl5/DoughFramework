using System.Collections.Generic;
using System.Linq;

public class EntityState : State<Entity>
{
    public static StateType Create<StateType>(Entity entity) where StateType : EntityState, new()
    {
        StateType stateType = new();
        stateType.SetOwner(entity);

        return stateType;
    }
}
