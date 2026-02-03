using System;
using System.Collections.Generic;
using System.Reflection;

public sealed class CommandAbility : Ability
{
    readonly Queue<ICommand> commandQueue = new();
    readonly Dictionary<Type, List<Action<ICommand>>> commandHandlers = new();
    readonly Dictionary<Ability, List<(Type type, Action<ICommand> invoker)>> abilityRegistrations = new();
    bool isDraining;
    readonly Queue<ICommand> outputQueue = new();
    public event Action OutputAvailable;

    public void Execute(ICommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        commandQueue.Enqueue(command);
        if (!isDraining)
        {
            DrainCommands();
        }
    }

    void DrainCommands()
    {
        isDraining = true;
        try
        {
            while (commandQueue.Count > 0)
            {
                var cmd = commandQueue.Dequeue();
                var type = cmd.GetType();

                if (!commandHandlers.TryGetValue(type, out var handlers) || handlers.Count == 0)
                {
                    continue; // no handlers: ignore silently
                }

                for (int i = 0; i < handlers.Count; i++)
                {
                    var h = handlers[i];
                    try
                    {
                        h(cmd);
                    }
                    catch
                    {
                        // Swallow to avoid Unity dependency for logging in Core
                        break; // stop remaining handlers for this command
                    }
                }
            }
        }
        finally
        {
            isDraining = false;
        }
    }

    internal void OnOwnerAbilityAdded(Ability ability)
    {
        if (ability == null || ReferenceEquals(ability, this))
        {
            return;
        }

        var handlers = AbilityCommandCache.GetHandlers(ability.GetType());
        if (handlers == null || handlers.Length == 0)
        {
            return;
        }

        if (!abilityRegistrations.TryGetValue(ability, out var registrations))
        {
            registrations = new();
            abilityRegistrations[ability] = registrations;
        }

        for (int i = 0; i < handlers.Length; i++)
        {
            var info = handlers[i];
            if (!commandHandlers.TryGetValue(info.CommandType, out var list))
            {
                list = new();
                commandHandlers[info.CommandType] = list;
            }

            Action<ICommand> invoker = CreateInvoker(ability, info.Method, info.CommandType);

            list.Add(invoker);
            registrations.Add((info.CommandType, invoker));
        }
    }

    internal void OnOwnerAbilityRemoved(Ability ability)
    {
        if (ability == null || !abilityRegistrations.TryGetValue(ability, out var registrations))
        {
            return;
        }

        for (int i = 0; i < registrations.Count; i++)
        {
            var entry = registrations[i];
            if (commandHandlers.TryGetValue(entry.type, out var list))
            {
                list.Remove(entry.invoker);
                if (list.Count == 0)
                {
                    commandHandlers.Remove(entry.type);
                }
            }
        }

        abilityRegistrations.Remove(ability);
    }

    static Action<ICommand> CreateInvoker(Ability ability, MethodInfo method, Type commandType)
    {
        var m = typeof(CommandAbility).GetMethod(nameof(CreateInvokerGeneric), BindingFlags.NonPublic | BindingFlags.Static);
        var g = m.MakeGenericMethod(commandType);
        return (Action<ICommand>)g.Invoke(null, new object[] { ability, method });
    }

    static Action<ICommand> CreateInvokerGeneric<T>(Ability ability, MethodInfo method)
    {
        var typed = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), ability, method, throwOnBindFailure: true);
        return cmd => typed((T)cmd);
    }

    public void EmitOutput(ICommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }
        outputQueue.Enqueue(command);
        OutputAvailable?.Invoke();
    }

    public bool TryDequeueOutput(out ICommand command)
    {
        if (outputQueue.Count == 0)
        {
            command = default;
            return false;
        }
        command = outputQueue.Dequeue();
        return true;
    }
}
