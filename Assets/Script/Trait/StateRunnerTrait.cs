using UnityEngine;

public class StateRunnerTrait : Trait
{
    public ActorStateBehaviour StateBehaviour => stateBehaviour;
    
    ActorStateBehaviour stateBehaviour;
    
    void Update()
    {
        if (stateBehaviour != null)
        {
            stateBehaviour.Update();
        }
    }

    void FixedUpdate()
    {
        if (stateBehaviour != null)
        {
            stateBehaviour.FixedUpdate();
        }
    }

    public void SetStateBehaviour(ActorStateBehaviour actorStateBehaviour)
    {
        this.stateBehaviour = actorStateBehaviour;
    }
}
