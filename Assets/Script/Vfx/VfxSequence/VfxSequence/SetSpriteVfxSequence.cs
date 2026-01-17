using UnityEngine;

public class SetSpriteVfxSequence : VfxSequence
{
    [SerializeField] SpriteRenderer spriteRenderer;
    
    SpriteSequenceProvider spriteSequenceProvider;
    
    protected override void Awake()
    {
        base.Awake();

        spriteSequenceProvider = GetComponentInParent<SpriteSequenceProvider>();
    }

    public override void StartSequence()
    {
        base.StartSequence();

        spriteRenderer.sprite = spriteSequenceProvider.Sprite;
    }
}
