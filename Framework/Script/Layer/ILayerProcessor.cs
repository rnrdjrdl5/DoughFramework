public interface ILayerProcessor<TInput>
{
    LayerResult ProcessInput(TInput input);
}
