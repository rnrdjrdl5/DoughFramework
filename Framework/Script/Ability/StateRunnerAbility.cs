using UnityEngine;

public class StateRunnerAbility : Ability
{
    public EntityStateBehaviour StateBehaviour => stateBehaviour;
    
    EntityStateBehaviour stateBehaviour;
    
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

    public void SetStateBehaviour(EntityStateBehaviour entityStateBehaviour)
    {
        this.stateBehaviour = entityStateBehaviour;
    }
}
