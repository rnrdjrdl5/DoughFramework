using UnityEngine;

[RequireComponent(typeof(Canvas))]
public sealed class UISortingOrder : MonoBehaviour
{
    [SerializeField] UILayer layer = UILayer.Core;
    [SerializeField] int overlayIndex = 0; // Panel/Modal 스택 인덱스
    [SerializeField] int extraOffset = 0;

    Canvas canvas;

    void Reset()
    {
        canvas = GetComponent<Canvas>();
        Apply();
    }

    void Awake()
    {
        if (canvas == null) canvas = GetComponent<Canvas>();
        Apply();
    }

    void OnValidate()
    {
        if (canvas == null) canvas = GetComponent<Canvas>();
        Apply();
    }

    void Apply()
    {
        if (canvas == null) return;
        canvas.overrideSorting = true;
        canvas.sortingOrder = UILayerOrder.Compute(layer, overlayIndex, extraOffset);
    }
}
