using UnityEngine;

public class ProcessorState : State<Actor>
{
    public Processor Processor => processor;
    public Actor Actor => processor.Actor;
    public StateRunnerTrait StateRunnerTrait => stateRunnerTrait; 

    Processor processor;
    StateRunnerTrait stateRunnerTrait;
    
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
        
        stateRunnerTrait = processor.Actor.GetTrait<StateRunnerTrait>();

        return this;
    }
}