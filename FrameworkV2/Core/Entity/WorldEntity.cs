using System;
using System.Collections.Generic;

[Ability(typeof(CommandAbility))]
public class WorldEntity : Entity
{
    public WorldEntity() : base()
    {
    }

    protected override void OnAbilityAdded(Ability ability)
    {
        base.OnAbilityAdded(ability);
        var cmd = GetAbility<CommandAbility>();
        cmd?.OnOwnerAbilityAdded(ability);
    }

    protected override void OnAbilityRemoved(Ability ability)
    {
        base.OnAbilityRemoved(ability);
        var cmd = GetAbility<CommandAbility>();
        cmd?.OnOwnerAbilityRemoved(ability);
    }

    public void Execute(ICommand command)
    {
        var cmd = GetAbility<CommandAbility>();
        if (cmd == null)
        {
            AddAbility(cmd = new CommandAbility());
        }
        cmd.Execute(command);
    }
}
