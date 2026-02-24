using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TraitSet
{
    public List<Trait> Traits => traits;
    
    List<Trait> traits = new();
    
    public TraitType GetTrait<TraitType>() where TraitType : Trait
    {
        return traits.Where(trait => typeof(TraitType).IsAssignableFrom(trait.GetType()))
            .Cast<TraitType>()
            .FirstOrDefault();
    }
}
