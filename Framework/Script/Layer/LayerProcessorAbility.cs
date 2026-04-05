public abstract class LayerProcessorAbility<TInput> : ProcessorAbility
{
    public LayerStack<TInput> LayerStack => layerStack;
    public LayerStackRunner<TInput> LayerStackRunner => layerStackRunner;

    LayerStack<TInput> layerStack;
    LayerStackRunner<TInput> layerStackRunner;

    public override void Initialize(IInitData initData = null)
    {
        initData ??= EmptyInitData.Instance;

        layerStack = new LayerStack<TInput>();
        layerStackRunner = new LayerStackRunner<TInput>(layerStack);

        base.Initialize(initData);
    }

    public override void Uninitialize()
    {
        layerStackRunner = null;
        layerStack?.Clear();
        layerStack = null;

        base.Uninitialize();
    }

    public void PushLayer(ILayerProcessor<TInput> layerProcessor)
    {
        layerStack?.PushLayer(layerProcessor);
    }

    public bool RemoveLayer(ILayerProcessor<TInput> layerProcessor)
    {
        return layerStack?.RemoveLayer(layerProcessor) ?? false;
    }

    public bool ContainsLayer(ILayerProcessor<TInput> layerProcessor)
    {
        return layerStack?.ContainsLayer(layerProcessor) ?? false;
    }

    public bool TryGetTopLayer(out ILayerProcessor<TInput> layerProcessor)
    {
        if (layerStack != null)
        {
            return layerStack.TryGetTopLayer(out layerProcessor);
        }

        layerProcessor = default;
        return false;
    }

    public LayerResult ProcessInput(TInput input)
    {
        return layerStackRunner?.ProcessInput(input) ?? LayerResult.Pass;
    }
}
