using System.Collections.Generic;

public class TeamTrait : Trait
{
    public int TeamId { get; set; }
    
    List<TeamTrait> alliance = new();
    
    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);
        
        alliance.Clear();
    }

    public bool TryAddAlliance(TeamTrait teamTrait)
    {
        if (alliance.Contains(teamTrait))
        {
            return false;
        }
        
        alliance.Add(teamTrait);
        
        return true;
    }

    public bool TryRemoveAlliance(TeamTrait teamTrait)
    {
        if (!alliance.Contains(teamTrait))
        {
            return false;
        }

        alliance.Remove(teamTrait);
        
        return true;
    }

    public bool IsAlly(Actor targetActor)
    {
        var teamTrait = targetActor.GetTrait<TeamTrait>();
        if (teamTrait == null)
        {
            return false;
        }

        return IsAlly(teamTrait);
    }

    public bool IsAlly(TeamTrait teamTrait)
    {
        return alliance.Contains(teamTrait);
    }

    public bool IsTeammate(Actor targetActor)
    {
        var teamTrait = targetActor.GetTrait<TeamTrait>();
        if (teamTrait == null)
        {
            return false;
        }

        return IsTeammate(teamTrait);
    }

    public bool IsTeammate(TeamTrait teamTrait)
    {
        return teamTrait.TeamId == TeamId;
    }
}