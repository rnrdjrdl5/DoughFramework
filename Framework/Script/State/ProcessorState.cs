using UnityEngine;

public class ProcessorState : State<Entity>
{
    public Processor Processor => processor;
    public Entity Entity => processor.Entity;
    public StateRunnerAbility StateRunnerAbility => stateRunnerAbility; 

    Processor processor;
    StateRunnerAbility stateRunnerAbility;
    
    public static StateType Create<StateType>(Entity entity, Processor processor) where StateType : ProcessorState, new()
    {
        StateType stateType = new();
        stateType.SetOwner(entity);
        stateType.SetProcessor(processor);

        return stateType;
    }

    public ProcessorState SetProcessor(Processor processor)
    {
        this.processor = processor;
        
        stateRunnerAbility = processor.Entity.GetAbility<StateRunnerAbility>();

        return this;
    }
}