using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProcesserFlow : Flow
{
    public Processer Processer => processer;
    public Actor Actor => processer.Actor;

    Processer processer;

    public ProcesserFlow SetProcesser(Processer processer)
    {
        this.processer = processer;

        return this;
    }
    public FlowType AddChild<FlowType>(Processer processer) where FlowType : ProcesserFlow, new()
    {
        FlowType flow = new();

        children.Add(flow);

        flow.parent = this;
        flow.SetOwner(flow.Owner);
        flow.SetProcesser(processer);
        
        flow.OnAddFlow();

        return flow;
    }
}
