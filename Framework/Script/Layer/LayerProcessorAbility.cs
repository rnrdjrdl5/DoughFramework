public abstract class LayerProcessorAbility<TLayer, TInput> : ProcessorAbility
    where TLayer : struct, System.Enum
{
    public LayerStack<TLayer> LayerStack => layerStack;
    public LayerStackRunner<TLayer, TInput> LayerStackRunner => layerStackRunner;

    LayerStack<TLayer> layerStack;
    LayerStackRunner<TLayer, TInput> layerStackRunner;

    public override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;

        layerStack = new LayerStack<TLayer>();
        layerStackRunner = new LayerStackRunner<TLayer, TInput>(layerStack);

        base.Initialize(initData);
    }

    public override void Uninitialize()
    {
        layerStackRunner = null;
        layerStack?.Clear();
        layerStack = null;

        base.Uninitialize();
    }

    public void PushLayer(TLayer layerType)
    {
        layerStack?.PushLayer(layerType);
    }

    public bool RemoveLayer(TLayer layerType)
    {
        return layerStack?.RemoveLayer(layerType) ?? false;
    }

    public bool ContainsLayer(TLayer layerType)
    {
        return layerStack?.ContainsLayer(layerType) ?? false;
    }

    public bool TryGetTopLayer(out TLayer layerType)
    {
        if (layerStack != null)
        {
            return layerStack.TryGetTopLayer(out layerType);
        }

        layerType = default;
        return false;
    }

    public void RegisterLayerProcessor(ILayerProcessor<TLayer, TInput> layerProcessor)
    {
        layerStackRunner?.RegisterProcessor(layerProcessor);
    }

    public bool UnregisterLayerProcessor(ILayerProcessor<TLayer, TInput> layerProcessor)
    {
        return layerStackRunner?.UnregisterProcessor(layerProcessor) ?? false;
    }

    public LayerResult ProcessInput(TInput input)
    {
        return layerStackRunner?.ProcessInput(input) ?? LayerResult.Pass;
    }
}
