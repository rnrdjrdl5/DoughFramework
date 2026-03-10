using System.Linq;

public partial class ProcessorAbility
{
    public Processor AddProcessor<ProcessorType>() where ProcessorType : Processor, new()
    {
        var processor = Processor.Create<ProcessorType>(Entity, this);
        if (processor is UpdateProcessor updateProcessor)
        {
            AddProcessor(updateProcessor);
        }
        else
        {
            AddProcessor(processor);
        }

        return processor;
    }
    
    public void AddProcessor(Processor processor)
    {
        processorSet.AddProcessor(processor, this);
    }
    
    public void AddProcessor(UpdateProcessor processor)
    {
        updateProcessorSet.AddProcessor(processor, this);
    }
    
    public bool RemoveProcessor<ProcessorType>() where ProcessorType : Processor
    {
        var processor = GetProcessor<ProcessorType>();
        if (processor == null)
        {
            return false;
        }
        
        if (processor is UpdateProcessor updateProcessor)
        {
            updateProcessorSet.RemoveProcessor(updateProcessor);
        }
        else
        {
            processorSet.RemoveProcessor(processor);
        }
        
        return true;
    }

    public bool RemoveProcessor(UpdateProcessor updateProcessor)
    {
        return updateProcessorSet.RemoveProcessor(updateProcessor);
    }
    
    public bool RemoveProcessor(Processor processor)
    {
        return processorSet.RemoveProcessor(processor);
    }

    public ProcessorType GetProcessor<ProcessorType>() where ProcessorType : Processor
    {
        var processor = updateProcessorSet.Processors.FirstOrDefault(updateProcessor => typeof(ProcessorType).IsAssignableFrom(updateProcessor.GetType()));
        if (processor != null)
        {
            return processor as ProcessorType;
        }

        var updateProcessor = processorSet.Processors.FirstOrDefault(updateProcessor => typeof(ProcessorType).IsAssignableFrom(updateProcessor.GetType()));
        if (updateProcessor != null)
        {
            return updateProcessor as ProcessorType;
        }

        return null;
    }
}