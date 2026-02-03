using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class Presence : MonoBehaviour, IPresence
{
    readonly Dictionary<Type, List<Action<ICommand>>> outputHandlers = new();
    WorldEntity entity;
    CommandAbility boundCommand;
    IServiceResolver services;
    
    bool isInitialized;
    bool isReady;
    bool isOutputDraining;

    public bool IsInitialized => isInitialized;
    public bool IsReady => isReady;

    public WorldEntity Entity { get; private set; }
    protected IServiceResolver Services => services;

    public void AttachService(IServiceResolver resolver)
    {
        services = resolver;
    }

    public void DetachService()
    {
        services = null;
    }

    public void BindEntity(WorldEntity entity)
    {
        Entity = entity;
        if (isReady)
        {
            UnbindOutput();
            TryBindOutput();
        }
    }

    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }
        OnInitialize();
        isInitialized = true;
    }

    public void Ready()
    {
        if (!isInitialized || isReady)
        {
            return;
        }
        OnReady();
        isReady = true;
        TryBindOutput();
    }

    public void Uninitialize()
    {
        if (!isInitialized)
        {
            return;
        }
        UnbindOutput();
        OnUninitialize();
        isReady = false;
        isInitialized = false;
    }

    protected virtual void OnInitialize()
    {
    }

    protected virtual void OnReady()
    {
    }

    protected virtual void OnUninitialize()
    {
    }

    void TryBindOutput()
    {
        entity = Entity;
        if (entity == null)
        {
            return;
        }

        BuildOutputHandlers();
        boundCommand = entity.GetAbility<CommandAbility>();
        if (boundCommand != null)
        {
            boundCommand.OutputAvailable += OnEntityOutputAvailable;
            DrainOutput();
        }
    }

    void UnbindOutput()
    {
        if (boundCommand != null)
        {
            boundCommand.OutputAvailable -= OnEntityOutputAvailable;
            boundCommand = null;
        }
        entity = null;
    }

    void OnEntityOutputAvailable()
    {
        DrainOutput();
    }

    void BuildOutputHandlers()
    {
        outputHandlers.Clear();
        var infos = PresenceOutputCommandCache.GetHandlers(GetType());
        if (infos == null || infos.Length == 0)
        {
            return;
        }

        for (int i = 0; i < infos.Length; i++)
        {
            var info = infos[i];
            if (!outputHandlers.TryGetValue(info.CommandType, out var list))
            {
                list = new();
                outputHandlers[info.CommandType] = list;
            }

            var invoker = CreateOutputInvoker(this, info.Method, info.CommandType);
            list.Add(invoker);
        }
    }

    static Action<ICommand> CreateOutputInvoker(object target, MethodInfo method, Type commandType)
    {
        var m = typeof(Presence).GetMethod(nameof(CreateOutputInvokerGeneric), BindingFlags.NonPublic | BindingFlags.Static);
        var g = m.MakeGenericMethod(commandType);
        return (Action<ICommand>)g.Invoke(null, new object[] { target, method });
    }

    static Action<ICommand> CreateOutputInvokerGeneric<T>(object target, MethodInfo method)
    {
        var typed = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), target, method, throwOnBindFailure: true);
        return cmd => typed((T)cmd);
    }

    void DrainOutput()
    {
        if (boundCommand == null)
        {
            return;
        }
        if (isOutputDraining)
        {
            return;
        }

        isOutputDraining = true;
        try
        {
            while (boundCommand.TryDequeueOutput(out var cmd))
            {
                var type = cmd.GetType();
                if (!outputHandlers.TryGetValue(type, out var handlers) || handlers.Count == 0)
                {
                    continue;
                }

                for (int i = 0; i < handlers.Count; i++)
                {
                    var h = handlers[i];
                    try
                    {
                        h(cmd);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                        break;
                    }
                }
            }
        }
        finally
        {
            isOutputDraining = false;
        }
    }

    protected virtual void Update()
    {
        DrainOutput();
    }

    protected virtual void OnDestroy()
    {
        Uninitialize();
        services = null;
    }
}
