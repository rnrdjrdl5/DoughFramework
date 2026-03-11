using System;
using System.Collections.Generic;
using System.Linq;

public partial class ProcessorAbility : Ability
{
    ProcessorSet<Processor> processorSet = new();
    ProcessorSet<UpdateProcessor> updateProcessorSet = new();

    public override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;
        base.Initialize(initData);
        
        var processorTypes = GetType()
            .GetCustomAttributes(typeof(ProcessorAttribute), true)
            .Cast<ProcessorAttribute>()
            .Select(attr => attr.Type);
        
        processorSet.Clear();
        
        foreach (var processorType in processorTypes)
        {
            var processor = Processor.Create(processorType, Entity, this);
            if (processor == null)
            {
                continue;
            }
            
            AddProcessor(processor, initData);
        }
        
        var updateProcessorTypes = GetType()
            .GetCustomAttributes(typeof(UpdateProcessorAttribute), true)
            .Cast<UpdateProcessorAttribute>()
            .Select(attr => attr.Type);
        
        updateProcessorSet.Clear();
        
        foreach (var updateProcessorType in updateProcessorTypes)
        {
            if (Processor.Create(updateProcessorType, Entity, this) is not UpdateProcessor updateProcessor)
            {
                continue;
            }

            AddProcessor(updateProcessor, initData);
        }
    }

    public override void Uninitialize()
    {
        for (var i = processorSet.Processors.Count - 1; i >= 0; i--)
        {
            processorSet.RemoveProcessor(processorSet.Processors[i]);
        }
        
        for (var i = updateProcessorSet.Processors.Count - 1; i >= 0; i--)
        {
            updateProcessorSet.RemoveProcessor(updateProcessorSet.Processors[i]);
        }

        base.Uninitialize();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < updateProcessorSet.Processors.Count; i++)
        {
            updateProcessorSet.Processors[i].FixedUpdate();
        }
    }

    void Update()
    {
        for (int i = 0; i < updateProcessorSet.Processors.Count; i++)
        {
            updateProcessorSet.Processors[i].Update();
        }
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
