public abstract class BaseInputLayerProcessor : Processor, ILayerProcessor<InputContext>
{
    InputProcessorAbility inputProcessorAbility;

    public override void Ready()
    {
        base.Ready();

        inputProcessorAbility = GetInputProcessorAbility();
        inputProcessorAbility?.PushLayer(this);
    }

    public override void Uninitialize()
    {
        inputProcessorAbility?.RemoveLayer(this);
        inputProcessorAbility = null;

        base.Uninitialize();
    }

    protected virtual InputProcessorAbility GetInputProcessorAbility()
    {
        var result = Entity.GetAbility<InputProcessorAbility>();
        if (result != null)
        {
            return result;
        }

        var inputRealm = Entity.GetFromRoot<FrameworkInputRealm>();
        return inputRealm?.GetAbility<InputProcessorAbility>();
    }

    public abstract LayerResult ProcessInput(InputContext input);
}
