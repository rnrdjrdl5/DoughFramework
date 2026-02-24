using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProcessorFlow : Flow
{
    public Processor Processor => processor;
    public Actor Actor => processor.Actor;

    Processor processor;

    public ProcessorFlow SetProcessor(Processor processor)
    {
        this.processor = processor;

        return this;
    }
    public FlowType AddChild<FlowType>(Processor processor) where FlowType : ProcessorFlow, new()
    {
        FlowType flow = new();

        children.Add(flow);

        flow.parent = this;
        flow.SetOwner(flow.Owner);
        flow.SetProcessor(processor);
        
        flow.OnAddFlow();

        return flow;
    }
}
