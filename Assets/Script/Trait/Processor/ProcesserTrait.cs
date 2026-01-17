using System;
using System.Collections.Generic;
using System.Linq;

public class ProcesserTrait : Trait
{
    List<Processer> processers = new();
    List<UpdateProcesser> updateProcessers = new();

    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);
        
        var processerTypes = GetType()
            .GetCustomAttributes(typeof(ProcesserAttribute), true)
            .Cast<ProcesserAttribute>()
            .Select(attr => attr.Type);
        
        processers.Clear();
        
        foreach (var processerType in processerTypes)
        {
            var processer = Processer.Create(processerType, Actor, this);
            if (processer == null)
            {
                continue;
            }

            processers.Add(processer);
        }
        
        var updateProcesserTypes = GetType()
            .GetCustomAttributes(typeof(UpdateProcesserAttribute), true)
            .Cast<UpdateProcesserAttribute>()
            .Select(attr => attr.Type);
        
        updateProcessers.Clear();
        
        foreach (var updateProcesserType in updateProcesserTypes)
        {
            if (Processer.Create(updateProcesserType, Actor, this) is not UpdateProcesser updateProcesser)
            {
                continue;
            }

            updateProcessers.Add(updateProcesser);
            updateProcesser.SetProcesserTrait(this);
            updateProcesser.Initialize();
        }

        foreach (var processor in processers)
        {
            processor.Ready();
        }

        foreach (var processor in updateProcessers)
        {
            processor.Ready();
        }
    }

    public override void Uninitialize()
    {
        foreach (var processer in processers)
        {
            processer.Uninitialize();
        }

        foreach (var updateProcesser in updateProcessers)
        {
            updateProcesser.Uninitialize();
        }

        base.Uninitialize();
    }

    void FixedUpdate()
    {
        foreach (var updateProcesser in updateProcessers)
        {
            updateProcesser.FixedUpdate();
        }
    }

    void Update()
    {
        foreach (var updateProcesser in updateProcessers)
        {
            updateProcesser.Update();
        }
    }

    public void AddProcesser(Processer processer)
    {
        processer.SetProcesserTrait(this);
        processers.Add(processer);
        
        processer.Initialize();
    }

    public void AddProcesser(UpdateProcesser processer)
    {
        updateProcessers.Add(processer);

        processer.Initialize();
    }

    public ProcesserType GetProcesser<ProcesserType>() where ProcesserType : Processer
    {
        var processer = updateProcessers.FirstOrDefault(updateProcesser => typeof(ProcesserType).IsAssignableFrom(updateProcesser.GetType()));
        if (processer != null)
        {
            return processer as ProcesserType;
        }

        var updateProcesser = processers.FirstOrDefault(updateProcesser => typeof(ProcesserType).IsAssignableFrom(updateProcesser.GetType()));
        if (updateProcesser != null)
        {
            return updateProcesser as ProcesserType;
        }

        return null;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class ProcesserAttribute : Attribute
{
    public Type Type;

    public ProcesserAttribute(Type type)
    {
        Type = type;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
public class UpdateProcesserAttribute : Attribute
{
    public Type Type;

    public UpdateProcesserAttribute(Type type)
    {
        Type = type;
    }
}