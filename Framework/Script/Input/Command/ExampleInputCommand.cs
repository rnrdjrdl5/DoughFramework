using UnityEngine;

public enum ExampleInputCommandType
{
    None,
    Move,
    Confirm,
}

public readonly struct ExampleInputCommand
{
    public readonly ExampleInputCommandType Type;
    public readonly Vector2 Axis;

    ExampleInputCommand(ExampleInputCommandType type, Vector2 axis = default)
    {
        Type = type;
        Axis = axis;
    }

    public static ExampleInputCommand CreateMove(Vector2 axis)
    {
        return new ExampleInputCommand(ExampleInputCommandType.Move, axis);
    }

    public static ExampleInputCommand CreateConfirm()
    {
        return new ExampleInputCommand(ExampleInputCommandType.Confirm);
    }
}

public class ExampleInputCommandMapper : IInputCommandMapper<ExampleInputCommand>
{
    public bool TryMap(InputContext input, out ExampleInputCommand command)
    {
        if (input.KeyCode == KeyCode.None)
        {
            command = ExampleInputCommand.CreateMove(input.Axis);
            return true;
        }

        if (input.StateType == InputStateType.Pressed && input.KeyCode == KeyCode.Space)
        {
            command = ExampleInputCommand.CreateConfirm();
            return true;
        }

        command = default;
        return false;
    }
}

public class ExampleInputCommandDispatcher : IInputCommandDispatcher<ExampleInputCommand>
{
    public LayerResult Dispatch(ExampleInputCommand command)
    {
        switch (command.Type)
        {
            case ExampleInputCommandType.Move:
                Debug.Log($"[ExampleInputCommand] Move axis={command.Axis}");
                return LayerResult.Consume;

            case ExampleInputCommandType.Confirm:
                Debug.Log("[ExampleInputCommand] Confirm");
                return LayerResult.Consume;

            default:
                return LayerResult.Pass;
        }
    }
}

public class ExampleInputCommandLayerProcessor : InputCommandLayerProcessor<ExampleInputCommand>
{
    readonly ExampleInputCommandMapper mapper = new();
    readonly ExampleInputCommandDispatcher dispatcher = new();

    protected override IInputCommandMapper<ExampleInputCommand> Mapper => mapper;
    protected override IInputCommandDispatcher<ExampleInputCommand> Dispatcher => dispatcher;
}
