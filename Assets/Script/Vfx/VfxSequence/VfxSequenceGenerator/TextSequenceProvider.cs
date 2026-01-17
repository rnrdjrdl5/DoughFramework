using System;
using UnityEngine;

public class TextSequenceProvider : MonoBehaviour
{
    public string VfxText => runtimeVfxText;
    
    [SerializeField] string vfxText;

    string runtimeVfxText;

    void Awake()
    {
        runtimeVfxText = vfxText;
    }

    public void SetVfxText(string text) => runtimeVfxText = text;
}
