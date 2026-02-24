using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilitySet
{
    public List<Ability> Abilities => abilities;
    
    List<Ability> abilities = new();
    
    public AbilityType GetAbility<AbilityType>() where AbilityType : Ability
    {
        return abilities.Where(ability => typeof(AbilityType).IsAssignableFrom(ability.GetType()))
            .Cast<AbilityType>()
            .FirstOrDefault();
    }
}
