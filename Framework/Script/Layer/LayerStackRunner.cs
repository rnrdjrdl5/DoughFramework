using System.Collections.Generic;

public class LayerStackRunner<TLayer, TInput>
    where TLayer : struct, System.Enum
{
    readonly Dictionary<TLayer, ILayerProcessor<TLayer, TInput>> registeredProcessors = new();
    readonly LayerStack<TLayer> activeLayerStack;

    public LayerStackRunner(LayerStack<TLayer> activeLayerStack)
    {
        this.activeLayerStack = activeLayerStack;
    }

    public void RegisterProcessor(ILayerProcessor<TLayer, TInput> layerProcessor)
    {
        if (layerProcessor == null)
        {
            return;
        }

        registeredProcessors[layerProcessor.LayerType] = layerProcessor;
    }

    public bool UnregisterProcessor(TLayer layerType)
    {
        return registeredProcessors.Remove(layerType);
    }

    public LayerResult ProcessInput(TInput input)
    {
        if (activeLayerStack == null)
        {
            return LayerResult.Pass;
        }

        foreach (var layerType in activeLayerStack.GetLayersTopFirst())
        {
            if (!registeredProcessors.TryGetValue(layerType, out var layerProcessor) || layerProcessor == null)
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
