public interface ILayerProcessor<TLayer, TInput>
    where TLayer : struct, System.Enum
{
    TLayer LayerType { get; }
    LayerResult ProcessInput(TInput input);
}
