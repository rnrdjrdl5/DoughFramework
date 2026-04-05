public class LayerStackRunner<TInput>
{
    readonly LayerStack<TInput> activeLayerStack;

    public LayerStackRunner(LayerStack<TInput> activeLayerStack)
    {
        this.activeLayerStack = activeLayerStack;
    }

    public LayerResult ProcessInput(TInput input)
    {
        if (activeLayerStack == null)
        {
            return LayerResult.Pass;
        }

        foreach (var layerProcessor in activeLayerStack.GetLayersTopFirst())
        {
            if (layerProcessor == null)
            {
                continue;
            }

            var result = layerProcessor.ProcessInput(input);
            if (result != LayerResult.Pass)
            {
                return result;
            }
        }

        return LayerResult.Pass;
    }
}
