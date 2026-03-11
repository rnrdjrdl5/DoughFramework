using System.Collections.Generic;

public class TeamAbility : Ability
{
    public int TeamId { get; set; }
    
    List<TeamAbility> alliance = new();
    
    public override void Initialize(IInitData initData = null)
    {
        base.Initialize(initData);
        
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

    public bool IsAlly(Entity targetEntity)
    {
        var teamAbility = targetEntity.GetAbility<TeamAbility>();
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

    public bool IsTeammate(Entity targetEntity)
    {
        var teamAbility = targetEntity.GetAbility<TeamAbility>();
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
