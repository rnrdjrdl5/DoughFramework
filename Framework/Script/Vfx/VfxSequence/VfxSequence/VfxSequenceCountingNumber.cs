using UnityEngine;

[RequireComponent(typeof(CountingNumberVfx))]
public class VfxSequenceCountingNumber : VfxSequence
{
    public int RuntimeStartCount => runtimeStartCount;
    public int RuntimeEndCount => runtimeEndCount;
    
    [SerializeField] CountingNumberVfx countingNumberVfx;

    [SerializeField] int startCount;
    [SerializeField] int endCount;

    int runtimeStartCount;
    int runtimeEndCount;

    public override void StartSequence()
    {
        base.StartSequence();
        
        countingNumberVfx.CountingAnimation(runtimeStartCount, runtimeEndCount, duration);
    }

    public void SetRuntimeStartCount(int count) => runtimeStartCount = count;
    public void SetRuntimeEndCount(int count) => runtimeEndCount = count;
}
