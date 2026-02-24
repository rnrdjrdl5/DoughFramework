using System.Collections.Generic;

public class TeamAbility : Ability
{
    public int TeamId { get; set; }
    
    List<TeamAbility> alliance = new();
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);
        
        alliance.Clear();
    }

    public bool TryAddAlliance(TeamAbility teamAbility)
    {
        if (alliance.Contains(teamAbility))
        {
            return false;
        }
        
        alliance.Add(teamAbility);
        
        return true;
    }

    public bool TryRemoveAlliance(TeamAbility teamAbility)
    {
        if (!alliance.Contains(teamAbility))
        {
            return false;
        }

        alliance.Remove(teamAbility);
        
        return true;
    }

    public bool IsAlly(Actor targetActor)
    {
        var teamAbility = targetActor.GetAbility<TeamAbility>();
        if (teamAbility == null)
        {
            return false;
        }

        return IsAlly(teamAbility);
    }

    public bool IsAlly(TeamAbility teamAbility)
    {
        return alliance.Contains(teamAbility);
    }

    public bool IsTeammate(Actor targetActor)
    {
        var teamAbility = targetActor.GetAbility<TeamAbility>();
        if (teamAbility == null)
        {
            return false;
        }

        return IsTeammate(teamAbility);
    }

    public bool IsTeammate(TeamAbility teamAbility)
    {
        return teamAbility.TeamId == TeamId;
    }
}