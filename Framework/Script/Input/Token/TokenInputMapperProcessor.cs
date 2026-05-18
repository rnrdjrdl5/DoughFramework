public class TokenInputMapperProcessor : Processor, IRawInputRequester
{
    InputCollectorAbility inputCollectorAbility;
    TokenInputRouterProcessor tokenInputRouterProcessor;
    TokenInputBindingData tokenInputBindingData;

    public override void Ready()
    {
        base.Ready();

        inputCollectorAbility = Entity.GetAbility<InputCollectorAbility>();
        if (inputCollectorAbility == null)
        {
            return;
        }
        
        inputCollectorAbility.SetRawInputRequester(this);

        tokenInputBindingData = Entity.GetEntityData<TokenInputBindingData>();
        inputCollectorAbility.SetInputBindingData(tokenInputBindingData);
        inputCollectorAbility.SetInputStateData(Entity.GetEntityData<RawInputStateData>());

        var tokenInputProcessorAbility = Entity.GetAbility<TokenInputProcessorAbility>();
        tokenInputRouterProcessor = tokenInputProcessorAbility?.GetProcessor<TokenInputRouterProcessor>();
    }

    public override void Uninitialize()
    {
        inputCollectorAbility?.SetRawInputRequester(null);
        inputCollectorAbility?.SetInputBindingData(null);
        inputCollectorAbility?.SetInputStateData(null);

        inputCollectorAbility = null;
        tokenInputRouterProcessor = null;
        tokenInputBindingData = null;

        base.Uninitialize();
    }

    public void RequestRawInput(RawInputContext rawInput)
    {
        if (TryMapInput(rawInput, out var tokenInput))
        {
            tokenInputRouterProcessor?.RouteInput(tokenInput);
        }
    }

    public bool TryMapInput(RawInputContext rawInput, out TokenInputContext tokenInput)
    {
        if (tokenInputBindingData != null && tokenInputBindingData.TryGetInputType(rawInput, out var inputType))
        {
            tokenInput = new TokenInputContext(inputType, rawInput);
            return true;
        }

        tokenInput = default;
        return false;
    }
}
