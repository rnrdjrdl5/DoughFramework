using System;
using UnityEngine;

// Sequence, Vfx를 처리하는 곳
public abstract class VfxSequence : MonoBehaviour
{
    public event Action onStart;
    public event Action onFinish;

    public VfxSequenceStatusType StatusType => statusType;
    public int ProcessOrder => processOrder;
    
    protected VfxSequenceObject sequenceObject;
    
    [SerializeField] protected int processOrder;
    [SerializeField] protected float duration;
    
    VfxSequenceStatusType statusType;
    float elapsedTime;

    protected virtual void Awake()
    {
        sequenceObject = GetComponent<VfxSequenceObject>();
    }

    protected virtual void OnEnable()
    {
        statusType = VfxSequenceStatusType.None;
    }

    public virtual void UpdateSequence(float deltaTime)
    {
        if (statusType != VfxSequenceStatusType.Playing)
        {
            return;
        }
        
        elapsedTime += deltaTime;
        ProcessSequence(elapsedTime);
        
        if (elapsedTime >= duration)
        {
            FinishSequence();
        }
    }
    
    public virtual void StartSequence()
    {
        statusType = VfxSequenceStatusType.Playing;
        elapsedTime = 0.0f;
        
        sequenceObject.UpdateProgressPosition();
    }
    
    protected virtual void FinishSequence()
    {
        statusType = VfxSequenceStatusType.Finish;
        onFinish?.Invoke();
    }
    
    protected virtual void ProcessSequence(float elapsedTime) { }
}
