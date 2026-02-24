using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class VfxSequenceGenerator : MonoBehaviour
{
    public event Action onFinish;
    
    [SerializeField] AllocGameObject vfxSequenceAllocator;
    [SerializeField] bool ignoreTimeScale = true;

    public void Play(Vector3 startPosition, Vector3 endPosition, int count, float duration)
    {
        UniTask.Void(async () =>
        {
            var delay = duration / count;
            var progressCount = 0;
            for (int i = 0; i < count; i++)
            {
                var createdObject = vfxSequenceAllocator.AllocateObject();
                
                var sequenceObject = createdObject.GetComponent<VfxSequenceObject>();
                sequenceObject.StartPosition = startPosition;
                sequenceObject.EndPosition = endPosition;

                var sequenceProcess = createdObject.GetComponent<VfxSequenceProcessor>();
                sequenceProcess.StartSequence();
                
                await UniTask.WaitForSeconds(delay, ignoreTimeScale: ignoreTimeScale, cancellationToken:this.GetCancellationTokenOnDestroy());

                sequenceProcess.onFinish += (order, maxOrder) =>
                {
                    if (order == maxOrder)
                    {
                        progressCount++;
                        if (progressCount == count)
                        {
                            vfxSequenceAllocator.DeallocateObjects();
                            onFinish?.Invoke();
                        }
                    }
                };
            }
        });
    }
}
