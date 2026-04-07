using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProcessorFlow : Flow
{
    public Processor Processor => processor;
    public Entity Entity => processor.Entity;
    public Realm Realm => processor.Realm;

    Processor processor;

    public ProcessorFlow SetProcessor(Processor processor)
    {
        this.processor = processor;

        return this;
    }

    public ProcessorType GetProcessor<ProcessorType>() where ProcessorType : Processor
    {
        return processor as ProcessorType;
    }
    
    public FlowType AddChild<FlowType>(Processor processor) where FlowType : ProcessorFlow, new()
    {
        FlowType flow = new();

        children.Add(flow);

        flow.parent = this;
        flow.SetOwner(Owner);
        flow.SetProcessor(processor);
        
        flow.OnAddFlow();

        return flow;
    }
}
