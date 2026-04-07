using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class FlowRunnerAbility : Ability
{
    public Flow Flow => flow;

    Flow flow;

    void Update()
    {
        if (flow != null)
        {
            flow.OnUpdateFlow();
        }
    }

    void FixedUpdate()
    {
        if (flow != null)
        {
            flow.OnFixedUpdateFlow();
        }
    }
    
    public void SetRootFlow(Flow flow)
    {
        this.flow?.OnExitFlow();
        this.flow = flow;
        
        this.flow?.OnAddFlow();
    }

    public FlowType SetRootFlow<FlowType>() where FlowType : Flow, new()
    {
        var rootFlow = Flow.Create<FlowType>(Entity);
        SetRootFlow(rootFlow);
        rootFlow.NextChildFlow();

        return rootFlow;
    }

    public FlowType SetRootProcessorFlow<FlowType>(Processor processor) where FlowType : ProcessorFlow, new()
    {
        var rootFlow = Flow.Create<FlowType>(Entity);
        rootFlow.SetProcessor(processor);
        SetRootFlow(rootFlow);
        rootFlow.NextChildFlow();

        return rootFlow;
    }
}
