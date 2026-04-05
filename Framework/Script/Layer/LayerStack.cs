using System.Collections.Generic;

public class LayerStack<TInput>
{
    readonly List<ILayerProcessor<TInput>> layers = new();

    public int Count => layers.Count;

    public void Clear()
    {
        layers.Clear();
    }

    public void PushLayer(ILayerProcessor<TInput> layerProcessor)
    {
        if (layerProcessor == null)
        {
            return;
        }

        RemoveLayer(layerProcessor);
        layers.Add(layerProcessor);
    }

    public bool RemoveLayer(ILayerProcessor<TInput> layerProcessor)
    {
        if (layerProcessor == null)
        {
            return false;
        }

        for (var i = layers.Count - 1; i >= 0; i--)
        {
            if (!ReferenceEquals(layers[i], layerProcessor))
            {
                continue;
            }

            layers.RemoveAt(i);
            return true;
        }

        return false;
    }

    public bool ContainsLayer(ILayerProcessor<TInput> layerProcessor)
    {
        if (layerProcessor == null)
        {
            return false;
        }

        for (var i = 0; i < layers.Count; i++)
        {
            if (ReferenceEquals(layers[i], layerProcessor))
            {
                return true;
            }
        }

        return false;
    }

    public bool TryGetTopLayer(out ILayerProcessor<TInput> layerProcessor)
    {
        if (layers.Count == 0)
        {
            layerProcessor = default;
            return false;
        }

        layerProcessor = layers[layers.Count - 1];
        return true;
    }

    public IEnumerable<ILayerProcessor<TInput>> GetLayersTopFirst()
    {
        for (var i = layers.Count - 1; i >= 0; i--)
        {
            yield return layers[i];
        }
    }
}
