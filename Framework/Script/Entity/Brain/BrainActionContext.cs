public class BrainActionContext
{
    public Brain Brain { get; private set; }
    public IControlled Controlled { get; private set; }
    public Entity ControlledEntity { get; private set; }
    public bool IsValid => ControlledEntity != null;

    public bool TryInitialize(Brain brain, IControlled controlled)
    {
        Reset();

        Brain = brain;
        Controlled = controlled;
        ControlledEntity = controlled as Entity;
        return ControlledEntity != null;
    }

    public bool Matches(IControlled controlled)
    {
        return Controlled == controlled;
    }

    public void Reset()
    {
        Brain = null;
        Controlled = null;
        ControlledEntity = null;
    }
}
