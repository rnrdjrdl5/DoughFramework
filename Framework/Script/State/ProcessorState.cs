using UnityEngine;

public class ProcessorState : State<Actor>
{
    public Processor Processor => processor;
    public Actor Actor => processor.Actor;
    public StateRunnerAbility StateRunnerAbility => stateRunnerAbility; 

    Processor processor;
    StateRunnerAbility stateRunnerAbility;
    
    public static StateType Create<StateType>(Actor actor, Processor processor) where StateType : ProcessorState, new()
    {
        StateType stateType = new();
        stateType.SetOwner(actor);
        stateType.SetProcessor(processor);

        return stateType;
    }

    public ProcessorState SetProcessor(Processor processor)
    {
        this.processor = processor;
        
        stateRunnerAbility = processor.Actor.GetAbility<StateRunnerAbility>();

        return this;
    }
}