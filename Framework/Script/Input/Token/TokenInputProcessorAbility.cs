using UnityEngine;

[Processor(typeof(TokenInputMapperProcessor))]
[Processor(typeof(TokenInputRouterProcessor))]
public abstract class TokenInputProcessorAbility : LayerProcessorAbility<TokenInputContext>
{
}
