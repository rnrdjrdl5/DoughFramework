public class TokenInputRouterProcessor : Processor
{
    TokenInputProcessorAbility tokenInputProcessorAbility;

    public override void Ready()
    {
        base.Ready();

        tokenInputProcessorAbility = Entity.GetAbility<TokenInputProcessorAbility>();
    }

    public override void Uninitialize()
    {
        tokenInputProcessorAbility = null;

        base.Uninitialize();
    }

    public LayerResult RouteInput(TokenInputContext tokenInput)
    {
        return tokenInputProcessorAbility?.ProcessInput(tokenInput) ?? LayerResult.Pass;
    }
}
