using System.Collections.Generic;
using System.Linq;

public class ProcessorSet<TProcessor>  where TProcessor : Processor, new()
{
    public IReadOnlyList<Processor> Processors => processors;

    List<TProcessor> processors = new();
    
    public void AddProcessor(TProcessor processor, ProcessorAbility processorAbility, Parameter parameter = null)
    {
        processor.SetProcessorAbility(processorAbility);
        processors.Add(processor);
        
        processor.Initialize(parameter);
        processor.Ready();
    }
    
    public Processor AddProcessor<ProcessorType>(Entity entity, ProcessorAbility processorAbility, Parameter parameter = null) where ProcessorType : TProcessor, new()
    {
        var processor = Processor.Create<ProcessorType>(entity, processorAbility) as TProcessor;
        AddProcessor(processor, processorAbility, parameter);

        return processor;
    }

    public bool RemoveProcessor<ProcessorType>() where ProcessorType : TProcessor
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
    
    public ProcessorType GetProcessor<ProcessorType>() where ProcessorType : TProcessor
    {
        var processor = processors.FirstOrDefault(updateProcessor => typeof(ProcessorType).IsAssignableFrom(updateProcessor.GetType()));
        return processor as ProcessorType;
    }

    public IEnumerable<ProcessorType> GetProcessors<ProcessorType>() where ProcessorType : TProcessor
    {
        var processor = processors.Where(updateProcessor => typeof(ProcessorType).IsAssignableFrom(updateProcessor.GetType()));
        return processor.Select(processor => processor as ProcessorType);
    }
    

    public void Clear()
    {
        processors.Clear();
    }
}