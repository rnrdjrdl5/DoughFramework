using System;
using System.Collections.Generic;
using System.Reflection;

// MonoBehaviour 없이 WorldEntity 출력 커맨드를 드레인하는 헤드리스 Presence
public class HeadlessPresence
{
    readonly Dictionary<Type, List<Action<ICommand>>> outputHandlers = new();
    WorldEntity boundEntity;
    CommandAbility boundCommand;
    bool isInitialized;
    bool isReady;
    bool isOutputDraining;

    public bool IsInitialized => isInitialized;
    public bool IsReady => isReady;
    public WorldEntity Entity { get; private set; }

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
        if (isInitialized) return;
        isInitialized = true;
    }

    public void Ready()
    {
        if (!isInitialized || isReady) return;
        isReady = true;
        TryBindOutput();
    }

    public void Uninitialize()
    {
        if (!isInitialized) return;
        UnbindOutput();
        isReady = false;
        isInitialized = false;
    }

    void TryBindOutput()
    {
        boundEntity = Entity;
        if (boundEntity == null) return;

        BuildOutputHandlers();
        boundCommand = boundEntity.GetAbility<CommandAbility>();
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
        boundEntity = null;
    }

    void OnEntityOutputAvailable()
    {
        DrainOutput();
    }

    void BuildOutputHandlers()
    {
        outputHandlers.Clear();
        var infos = PresenceOutputCommandCache.GetHandlers(GetType());
        if (infos == null || infos.Length == 0) return;

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
        var m = typeof(HeadlessPresence).GetMethod(nameof(CreateOutputInvokerGeneric), BindingFlags.NonPublic | BindingFlags.Static);
        var g = m.MakeGenericMethod(commandType);
        return (Action<ICommand>)g.Invoke(null, new object[] { target, method });
    }

    static Action<ICommand> CreateOutputInvokerGeneric<T>(object target, MethodInfo method)
    {
        var typed = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), target, method, throwOnBindFailure: true);
        return cmd => typed((T)cmd);
    }

    // 수동 호출용. 게임 루프가 없는 환경에서 필요 시 호출
    public void Tick()
    {
        DrainOutput();
    }

    void DrainOutput()
    {
        if (boundCommand == null) return;
        if (isOutputDraining) return;

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
                    catch
                    {
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
}

// 예제용: 출력 커맨드를 수집해 테스트에서 검증 가능하게 하는 간단 구현
public sealed class ExampleHeadlessPresence : HeadlessPresence
{
    public Vec3? LastPosition { get; private set; }
    public (int current, int max)? LastHealth { get; private set; }

    [HandlesOutputCommand(typeof(PositionUpdated))]
    void OnPositionUpdated(PositionUpdated cmd)
    {
        LastPosition = cmd.WorldPosition;
    }

    [HandlesOutputCommand(typeof(HealthChanged))]
    void OnHealthChanged(HealthChanged cmd)
    {
        LastHealth = (cmd.Current, cmd.Max);
    }
}

