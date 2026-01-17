using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

public class StateBehaviour
{
    public State ParentState => parentState;
    public State ActivatedState => activatedState;
    
    List<State> childStates = new();
    State parentState;
    State activatedState;
    
    public void SetParentState(State parentState)
    {
        this.parentState = parentState;
    }
    
    public StateType AddState<StateType>() where StateType : State, new()
    {
        StateType state = new();
        AddState(state);

        return state;
    }

    public State AddState(State state)
    {
        state.SetParent(parentState);
        childStates.Add(state);
        
        state.OnAddState();

        return state;
    }
    
    public State GetState<StateType>() where StateType : State => childStates.FirstOrDefault(state => typeof(StateType).IsAssignableFrom(state.GetType()));
    
    public virtual State ActivateState<StateType>() where StateType : State
    {
        var state = GetState<StateType>();
        if (state != null)
        {
            ActivateState(state);
        }

        return state;
    }

    public bool IsActivateState<StateType>() where StateType : State
    {
        return typeof(StateType).IsAssignableFrom(activatedState.GetType());
    }

    public void ActivateState(State state)
    {
        if (!childStates.Contains(state))
        {
            return;
        }
        
        if (activatedState != null)
        {
            activatedState.OnExitState();
        }

        activatedState = state;
        activatedState.OnEnterState();
        activatedState.OnEnterStateAsync().Forget();
    }

}

public class StateBehaviour<OwnerType> where OwnerType : new()
{
    List<State<OwnerType>> childStates = new();
    
    State<OwnerType> parentState;
    State<OwnerType> activatedState;
    
    public void SetParentState(State<OwnerType> parentState)
    {
        this.parentState = parentState;
        
        parentState.OnAddState();
    }
    
    public StateType AddState<StateType>() where StateType : State<OwnerType>, new()
    {
        StateType state = new();
        AddState(state);

        return state;
    }

    public State<OwnerType> AddState(State<OwnerType> state)
    {
        state.SetParent(parentState);
        state.SetOwner(state.Owner);
        
        childStates.Add(state);
        
        state.OnAddState();

        return state;
    }
    
    public State<OwnerType> GetState<StateType>() where StateType : State<OwnerType> => childStates.FirstOrDefault(state => typeof(StateType).IsAssignableFrom(state.GetType()));
    
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

    public void Update()
    {
        if (activatedState == null)
        {
            return;
        }
        
        activatedState.OnFixedUpdateState();
    }

    public void FixedUpdate()
    {
        if (activatedState == null)
        {
            return;
        }
        
        activatedState.OnFixedUpdateState();
    }
    
}