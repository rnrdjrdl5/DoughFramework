using System.Collections.Generic;

public class LayerStack<TLayer>
    where TLayer : struct, System.Enum
{
    readonly List<TLayer> layers = new();

    public int Count => layers.Count;

    public void Clear()
    {
        layers.Clear();
    }

    public void PushLayer(TLayer layerType)
    {
        RemoveLayer(layerType);
        layers.Add(layerType);
    }

    public bool RemoveLayer(TLayer layerType)
    {
        for (var i = layers.Count - 1; i >= 0; i--)
        {
            if (!EqualityComparer<TLayer>.Default.Equals(layers[i], layerType))
            {
                continue;
            }

            layers.RemoveAt(i);
            return true;
        }

        return false;
    }

    public bool ContainsLayer(TLayer layerType)
    {
        for (var i = 0; i < layers.Count; i++)
        {
            if (EqualityComparer<TLayer>.Default.Equals(layers[i], layerType))
            {
                return true;
            }
        }

        return false;
    }

    public bool TryGetTopLayer(out TLayer layerType)
    {
        if (layers.Count == 0)
        {
            layerType = default;
            return false;
        }

        layerType = layers[layers.Count - 1];
        return true;
    }

    public IEnumerable<TLayer> GetLayersTopFirst()
    {
        for (var i = layers.Count - 1; i >= 0; i--)
        {
            yield return layers[i];
        }
    }
}
