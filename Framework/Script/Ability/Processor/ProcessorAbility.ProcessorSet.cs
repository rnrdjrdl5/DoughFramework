using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class ProcessorAbility
{
    public ProcessorType AddProcessor<ProcessorType>(IInitData initData = null) where ProcessorType : Processor, new()
    {
        var processor = Processor.Create<ProcessorType>(Entity, this);
        if (processor is UpdateProcessor updateProcessor)
        {
            AddProcessor(updateProcessor, initData);
        }
        else
        {
            AddProcessor(processor, initData);
        }

        return processor;
    }
    
    public void AddProcessor(Processor processor, IInitData initData = null)
    {
        processorSet.AddProcessor(processor, this, initData);
    }
    
    public void AddProcessor(UpdateProcessor processor, IInitData initData = null)
    {
        updateProcessorSet.AddProcessor(processor, this, initData);
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

    public IEnumerable<ProcessorType> GetUpdateProcessors<ProcessorType>() where ProcessorType : UpdateProcessor
    {
        return updateProcessorSet.GetProcessors<ProcessorType>();
    }

    public IEnumerable<ProcessorType> GetProcessors<ProcessorType>() where ProcessorType : Processor
    {
        return processorSet.GetProcessors<ProcessorType>();
    }
}
