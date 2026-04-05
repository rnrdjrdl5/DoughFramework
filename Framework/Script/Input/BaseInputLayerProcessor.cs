public abstract class BaseInputLayerProcessor : Processor, ILayerProcessor<PhysicalInputTokenEvent>
{
    protected FrameworkInputProcessorAbility FrameworkInputProcessorAbility => frameworkInputProcessorAbility;

    FrameworkInputProcessorAbility frameworkInputProcessorAbility;

    public override void Ready()
    {
        base.Ready();

        frameworkInputProcessorAbility = GetInputProcessorAbility();
        frameworkInputProcessorAbility?.PushLayer(this);
    }

    public override void Uninitialize()
    {
        frameworkInputProcessorAbility?.RemoveLayer(this);
        frameworkInputProcessorAbility = null;

        base.Uninitialize();
    }

    protected virtual FrameworkInputProcessorAbility GetInputProcessorAbility()
    {
        var result = Entity.GetAbility<FrameworkInputProcessorAbility>();
        if (result != null)
        {
            return result;
        }

        var inputRealm = Entity.GetFromRoot<FrameworkInputRealm>();
        return inputRealm?.GetAbility<FrameworkInputProcessorAbility>();
    }

    public abstract LayerResult ProcessInput(PhysicalInputTokenEvent input);
}
