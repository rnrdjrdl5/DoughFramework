using System;

public sealed class OutputMovementAbility : Ability
{
    Vec3 currentPosition;

    [HandlesCommand(typeof(MoveCommand))]
    void OnMove(MoveCommand cmd)
    {
        var step = cmd.Direction.Normalized * (cmd.Speed * cmd.DeltaTime);
        currentPosition = currentPosition + step;
        var commandAbility = AbilityResolver?.GetAbility<CommandAbility>();
        commandAbility?.EmitOutput(new PositionUpdated { WorldPosition = currentPosition });
    }
}
