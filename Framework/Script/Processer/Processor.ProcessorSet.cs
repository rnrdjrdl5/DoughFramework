public partial class Processor
{
    public Processor AddProcessor<ProcessorType>() where ProcessorType : Processor, new()
    {
        var processor = Create<ProcessorType>(Entity, ProcessorAbility);
        AddProcessor(processor);
        
        return processor;
    }

    public Processor AddDynamicProcessor<ProcessorType>() where ProcessorType : Processor, new()
    {
        var processor = Create<ProcessorType>(Entity, ProcessorAbility);
        AddDynamicProcessor(processor);
        
        return processor;
    }
    
    public void AddProcessor(Processor processor)
    {
        processorSet.AddProcessor(processor, processorAbility);
    }
    
    public void AddDynamicProcessor(Processor processor)
    {
        processorSet.AddDynamicProcessor(processor, processorAbility);
    }
    
    
    public bool RemoveProcessor<ProcessorType>() where ProcessorType : Processor
    {
        var processor = GetProcessor<ProcessorType>();
        if (processor == null)
        {
            return false;
        }
        
        RemoveProcessor(processor);
        return true;
    }
    
    public bool RemoveProcessor(Processor processor)
    {
        return processorSet.RemoveProcessor(processor);
    }

    public ProcessorType GetProcessor<ProcessorType>() where ProcessorType : Processor
    {
        var processor = processorSet.GetProcessor<ProcessorType>() as ProcessorType;
        
        return processor;
    }
}
