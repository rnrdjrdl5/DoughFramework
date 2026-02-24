using System;
using System.Collections.Generic;
using System.Linq;

public class ProcessorAbility : Ability
{
    List<Processor> processors = new();
    List<UpdateProcessor> updateProcessors = new();

    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);
        
        var processorTypes = GetType()
            .GetCustomAttributes(typeof(ProcessorAttribute), true)
            .Cast<ProcessorAttribute>()
            .Select(attr => attr.Type);
        
        processors.Clear();
        
        foreach (var processorType in processorTypes)
        {
            var processor = Processor.Create(processorType, Entity, this);
            if (processor == null)
            {
                continue;
            }

            processors.Add(processor);
        }
        
        var updateProcessorTypes = GetType()
            .GetCustomAttributes(typeof(UpdateProcessorAttribute), true)
            .Cast<UpdateProcessorAttribute>()
            .Select(attr => attr.Type);
        
        updateProcessors.Clear();
        
        foreach (var updateProcessorType in updateProcessorTypes)
        {
            if (Processor.Create(updateProcessorType, Entity, this) is not UpdateProcessor updateProcessor)
            {
                continue;
            }

            updateProcessors.Add(updateProcessor);
            updateProcessor.SetProcessorAbility(this);
            updateProcessor.Initialize();
        }

        foreach (var processor in processors)
        {
            processor.Ready();
        }

        foreach (var processor in updateProcessors)
        {
            processor.Ready();
        }
    }

    public override void Uninitialize()
    {
        foreach (var processor in processors)
        {
            processor.Uninitialize();
        }

        foreach (var updateProcessor in updateProcessors)
        {
            updateProcessor.Uninitialize();
        }

        base.Uninitialize();
    }

    void FixedUpdate()
    {
        foreach (var updateProcessor in updateProcessors)
        {
            updateProcessor.FixedUpdate();
        }
    }

    void Update()
    {
        foreach (var updateProcessor in updateProcessors)
        {
            updateProcessor.Update();
        }
    }

    public void AddProcessor(Processor processor)
    {
        processor.SetProcessorAbility(this);
        processors.Add(processor);
        
        processor.Initialize();
    }

    public void AddProcessor(UpdateProcessor processor)
    {
        updateProcessors.Add(processor);

        processor.Initialize();
    }

    public ProcessorType GetProcessor<ProcessorType>() where ProcessorType : Processor
    {
        var processor = updateProcessors.FirstOrDefault(updateProcessor => typeof(ProcessorType).IsAssignableFrom(updateProcessor.GetType()));
        if (processor != null)
        {
            return processor as ProcessorType;
        }

        var updateProcessor = processors.FirstOrDefault(updateProcessor => typeof(ProcessorType).IsAssignableFrom(updateProcessor.GetType()));
        if (updateProcessor != null)
        {
            return updateProcessor as ProcessorType;
        }

        return null;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class ProcessorAttribute : Attribute
{
    public Type Type;

    public ProcessorAttribute(Type type)
    {
        Type = type;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class UpdateProcessorAttribute : Attribute
{
    public Type Type;

    public UpdateProcessorAttribute(Type type)
    {
        Type = type;
    }
}