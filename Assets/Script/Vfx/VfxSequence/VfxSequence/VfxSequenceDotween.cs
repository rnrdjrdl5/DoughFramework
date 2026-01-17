using DG.Tweening;
using UnityEngine;


[RequireComponent(typeof(DOTweenAnimation))]
public class VfxSequenceDotween : VfxSequence
{
    [SerializeField] DOTweenAnimation dotweenAnimation;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        dotweenAnimation.autoPlay = false;
        dotweenAnimation.autoKill = false;
    }

    public override void StartSequence()
    {
        base.StartSequence();
        
        dotweenAnimation.DORewind();
        dotweenAnimation.DOPlay();
    }
}
