using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

public class BaseState
{
    public virtual void OnAddState()
    {
        
    }

    public virtual void OnEnterState()
    {

    }

    public virtual void OnUpdateState()
    {
        
    }

    public virtual void OnFixedUpdateState()
    {

    }

    public virtual void OnExitState()
    {

    }

    public virtual async UniTask OnEnterStateAsync()
    {
        
    }
}

public class State : BaseState
{
    public State Parent => parent;
    
    State parent;

    public static State Create()
    {
        var state = new State();

        return state;
    }

    public void SetParent(State state)
    {
        this.parent = state;
    }
}

public class State<OwnerType> : BaseState where OwnerType : new()
{
    public OwnerType Owner => owner;
    public State<OwnerType> Parent => parent;
    
    OwnerType owner;
    State<OwnerType> parent;

    public static State<OwnerType> Create<OwnerType>(OwnerType ownerType) where OwnerType : new()
    {
        var state = new State<OwnerType>();
        state.SetOwner(ownerType);

        return state;
    }
    
    public void SetOwner(OwnerType owner)
    {
        this.owner = owner;
    }

    public void SetParent(State<OwnerType> state)
    {
        this.parent = state;
    }
}

public class SubState<OwnerType> : State<OwnerType> where OwnerType : new()
{
    List<State<OwnerType>> states = new();

    State<OwnerType> parent;
    State<OwnerType> activatedState;
    
    public StateType AddState<StateType>() where StateType : State<OwnerType>, new()
    {
        StateType state = new();
        
        AddState(state);

        return state;
    }

    public State<OwnerType> AddState(State<OwnerType> state)
    {
        states.Add(state);

        state.SetParent(this);
        state.SetOwner(state.Owner);
        
        state.OnAddState();

        return state;
    }

    public State<OwnerType> GetState<StateType>() where StateType : State<OwnerType> => states.FirstOrDefault(state => typeof(StateType).IsAssignableFrom(state.GetType()));
    
    public virtual State<OwnerType> ActivateState<StateType>() where StateType : State<OwnerType>
    {
        var state = GetState<StateType>();

        if (state != null)
        {
            ActivateState(state);
        }

        return state;
    }

    public bool IsActivateState<StateType>() where StateType : State<OwnerType>
    {
        return typeof(StateType).IsAssignableFrom(activatedState.GetType());
    }

    void ActivateState(State<OwnerType> state)
    {
        if (activatedState != null)
        {
            activatedState.OnExitState();
        }

        activatedState = state;
        
        activatedState.OnEnterState();
        activatedState.OnEnterStateAsync().Forget();
    }

    public override void OnUpdateState()
    {
        base.OnUpdateState();
        
        activatedState.OnUpdateState();
    }

    public override void OnFixedUpdateState()
    {
        base.OnFixedUpdateState();
        
        activatedState.OnFixedUpdateState();
    }
}