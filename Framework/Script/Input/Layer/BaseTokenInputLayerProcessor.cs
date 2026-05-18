public abstract class BaseTokenInputLayerProcessor : Processor, ILayerProcessor<TokenInputContext>
{
    protected TokenInputProcessorAbility TokenInputProcessorAbility => tokenInputProcessorAbility;

    TokenInputProcessorAbility tokenInputProcessorAbility;

    public override void Ready()
    {
        base.Ready();

        tokenInputProcessorAbility = GetInputProcessorAbility();
        tokenInputProcessorAbility?.PushLayer(this);
    }

    public override void Uninitialize()
    {
        tokenInputProcessorAbility?.RemoveLayer(this);
        tokenInputProcessorAbility = null;

        base.Uninitialize();
    }

    protected virtual TokenInputProcessorAbility GetInputProcessorAbility()
    {
        var result = Entity.GetAbility<TokenInputProcessorAbility>();
        if (result != null)
        {
            return result;
        }

        var inputRealm = Entity.GetFromRoot<FrameworkInputRealm>();
        return inputRealm?.GetAbility<TokenInputProcessorAbility>();
    }

    public abstract LayerResult ProcessInput(TokenInputContext input);
}
