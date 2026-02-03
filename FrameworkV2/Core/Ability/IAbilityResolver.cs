public interface IAbilityResolver
{
    bool HasAbility<T>() where T : Ability;
    T GetAbility<T>() where T : Ability;
}
