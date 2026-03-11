using System.Collections.Generic;
using System.Linq;

public class ProcessorSet<TProcessor>  where TProcessor : Processor, new()
{
    public IReadOnlyList<TProcessor> Processors => processors;

    List<TProcessor> processors = new();
    
    public void AddProcessor(TProcessor processor, ProcessorAbility processorAbility, IInitData initData = null)
    {
        processor.SetProcessorAbility(processorAbility);
        processors.Add(processor);
        
        processor.Initialize(initData);
    }
    
    public Processor AddProcessor<ProcessorType>(Entity entity, ProcessorAbility processorAbility, IInitData initData = null) where ProcessorType : TProcessor, new()
    {
        var processor = Processor.Create<ProcessorType>(entity, processorAbility) as TProcessor;
        AddProcessor(processor, processorAbility, initData);

        return processor;
    }

    public bool RemoveProcessor<ProcessorType>() where ProcessorType : TProcessor
    {
        var processor = GetProcessor<ProcessorType>();
        if (processor == null)
        {
            return false;
        }
        
        RemoveProcessor(processor);
        
        return true;
    }

    public bool RemoveProcessor(TProcessor processor)
    {
        if (processors.All(x => x != processor))
        {
            return false;
        }

        processor.Uninitialize();
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
