using TMPro;
using UnityEngine;

public class SetTextVfxSequence : VfxSequence
{
    [SerializeField] TMP_Text targetText;

    TextSequenceProvider textSequenceProvider;

    protected override void Awake()
    {
        base.Awake();
        
        textSequenceProvider = GetComponentInParent<TextSequenceProvider>();
    }

    public override void StartSequence()
    {
        base.StartSequence();
        
        targetText.text = textSequenceProvider.VfxText;
    }
}
