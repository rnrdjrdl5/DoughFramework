using System.Collections.Generic;
using System.Linq;

public class ProcessorSet<TProcessor>  where TProcessor : Processor, new()
{
    public IReadOnlyList<Processor> Processors => processors;

    List<TProcessor> processors = new();
    
    public void AddProcessor(TProcessor processor, ProcessorAbility processorAbility)
    {
        processor.SetProcessorAbility(processorAbility);
        processors.Add(processor);
        
        processor.Initialize();
    }
    
    public Processor AddProcessor<ProcessorType>(Entity entity, ProcessorAbility processorAbility) where ProcessorType : Processor, new()
    {
        var processor = Processor.Create<ProcessorType>(entity, processorAbility) as TProcessor;
        AddProcessor(processor, processorAbility);

        return processor;
    }

    public bool RemoveProcessor<ProcessorType>() where ProcessorType : Processor
    {
        var processor = GetProcessor<ProcessorType>();
        if (processor == null)
        {
            return false;
        }
        
        processors.Remove(processor);
        
        return true;
    }

    public bool RemoveProcessor(TProcessor processor)
    {
        if (processors.All(x => x != processor))
        {
            return false;
        }

        processors.Remove(processor);
        return true;
    }
    
    public TProcessor GetProcessor<ProcessorType>() where ProcessorType : Processor
    {
        var processor = processors.FirstOrDefault(updateProcessor => typeof(ProcessorType).IsAssignableFrom(updateProcessor.GetType()));
        return processor;
    }

    public void Clear()
    {
        processors.Clear();
    }
}