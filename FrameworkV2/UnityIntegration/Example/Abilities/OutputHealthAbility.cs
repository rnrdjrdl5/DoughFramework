using System;

public sealed class OutputHealthAbility : Ability
{
    int current = 100;
    int max = 100;

    [HandlesCommand(typeof(ApplyDamageCommand))]
    void OnApplyDamage(ApplyDamageCommand cmd)
    {
        var next = current - cmd.Amount;
        if (next < 0) next = 0;
        if (next > max) next = max;
        current = next;
        var commandAbility = AbilityResolver?.GetAbility<CommandAbility>();
        commandAbility?.EmitOutput(new HealthChanged { Current = current, Max = max });
    }
}
