using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flow : Flow<Actor>
{
    public event System.Action OnEnter;
    public Flow Parent => parent;
    
    protected List<Flow> children = new();
    protected Flow parent;
    protected Flow activatedChildFlow;

    bool isLoop;

    public void SetLoop(bool isLoop)
    {
        this.isLoop = isLoop;
    }

    public static FlowType Create<FlowType>(Actor actor) where FlowType : Flow, new()
    {
        FlowType flowType = new();
        flowType.SetOwner(actor);

        return flowType;
    }

    public FlowType AddChild<FlowType>() where FlowType : Flow, new()
    {
        FlowType flow = new();
        flow.parent = this;
        flow.SetOwner(flow.Owner);
        
        children.Add(flow);

        flow.OnAddFlow();

        return flow;
    }

    public Flow NextChildFlow()
    {
        if (activatedChildFlow == null)
        {
            if (children.Count > 0)
            {
                activatedChildFlow = children[0];
                
                activatedChildFlow.OnEnterFlow();
                activatedChildFlow.NextChildFlow();

                return activatedChildFlow;
            }
            else
            {
                return null;
            }
        }

        else
        {
            var result = activatedChildFlow.NextChildFlow();

            if (result == null)
            {
                var index = children.IndexOf(activatedChildFlow);

                if (children.Count <= index + 1)
                {
                    if (isLoop)
                    {
                        ActivateChildFlow(children[0]);

                        return activatedChildFlow;

                    }

                    else
                    {
                        return null;
                    }
                }

                else
                {
                    ActivateChildFlow(children[index + 1]);

                    return activatedChildFlow;
                }
            }

            else
            {
                return activatedChildFlow;
            }
        }
    }

    public void Finish()
    {
        if (parent != null)
        {
            parent.NextChildFlow();
        }
    }

    public Flow ActivateChildFlow<FlowType>() where FlowType : Flow
    {
        var flow = children.FirstOrDefault(flow => typeof(FlowType).IsAssignableFrom(flow.GetType()));

        if (flow != null)
        {
            ActivateChildFlow(flow);
        }

        return flow;
    }

    void ActivateChildFlow(Flow flow)
    {
        activatedChildFlow.OnExitFlow();

        activatedChildFlow = flow;
        activatedChildFlow.OnEnterFlow();
        
        activatedChildFlow.NextChildFlow();
    }

    public Flow GetRootFlow()
    {
        var flow = this;
        while (flow.parent != null)
        {
            flow = flow.parent;
        }

        return flow;
    }

    // NOTE : 같은 Flow를 2개 이상 사용할 경우 문제 발생
    public Flow GetFlowAll<FlowType>() where FlowType : Flow
    {
        var rootFlow = GetRootFlow();
        
        return rootFlow.GetFlowInChildren<FlowType>();
    }
    
    Flow GetFlowInChildren<FlowType>() where FlowType : Flow
    {
        if (typeof(FlowType).IsAssignableFrom(GetType()))
        {
            return this;
        }

        foreach (var child in children)
        {
            var childFlow = child.GetFlowInChildren<FlowType>();
            if (childFlow != null)
            {
                return childFlow;
            }
        }

        return null;
    }

    public bool IsActivateFlow<FlowType>() where FlowType : Flow
    {
        return typeof(FlowType) == activatedChildFlow.GetType();
    }
    public virtual void OnAddFlow()
    {
        
    }

    public virtual void OnEnterFlow()
    {
        OnEnter?.Invoke();
    }

    public virtual void OnUpdateFlow()
    {

    }

    public virtual void OnFixedUpdateFlow()
    {

    }

    public virtual void OnExitFlow()
    {

    }
}

public class Flow<OwnerType>
{
    public OwnerType Owner => owner;

    OwnerType owner;

    public void SetOwner(OwnerType owner)
    {
        this.owner = owner;
    }
}
