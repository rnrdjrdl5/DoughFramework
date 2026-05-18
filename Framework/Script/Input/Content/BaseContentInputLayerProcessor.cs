public abstract class BaseContentInputLayerProcessor<TInputType> : BaseTokenInputLayerProcessor
    where TInputType : struct
{
    public override LayerResult ProcessInput(TokenInputContext tokenInput)
    {
        if (!TryMapContentInput(tokenInput, out var contentInput))
        {
            return LayerResult.Pass;
        }

        return ProcessContentInput(contentInput);
    }

    protected abstract bool TryMapContentInput(
        TokenInputContext tokenInput,
        out ContentInputContext<TInputType> contentInput);

    protected abstract LayerResult ProcessContentInput(
        ContentInputContext<TInputType> contentInput);
}
