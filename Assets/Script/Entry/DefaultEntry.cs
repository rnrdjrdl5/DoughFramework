public class DefaultEntry : Entry
{
    protected override void Initialize()
    {
        base.Initialize();
        
        AddUniverse(Universe.Create<DefaultUniverse>());
    }
}