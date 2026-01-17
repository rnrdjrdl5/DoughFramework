using UnityEngine;

public class ProcesserState : State<Actor>
{
    public Processer Processer => processer;
    public Actor Actor => processer.Actor;
    public StateRunnerTrait StateRunnerTrait => stateRunnerTrait; 

    Processer processer;
    StateRunnerTrait stateRunnerTrait;
    
    public static StateType Create<StateType>(Actor actor, Processer processer) where StateType : ProcesserState, new()
    {
        StateType stateType = new();
        stateType.SetOwner(actor);
        stateType.SetProcesser(processer);

        return stateType;
    }

    public ProcesserState SetProcesser(Processer processer)
    {
        this.processer = processer;
        
        stateRunnerTrait = processer.Actor.GetTrait<StateRunnerTrait>();

        return this;
    }
}