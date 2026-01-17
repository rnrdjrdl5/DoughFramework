using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Sequence를 플레이하고 관리하는 곳
public class VfxSequenceProcesser : MonoBehaviour
{
    public event Action<int> onStart;
    public event Action<int,int> onFinish;
    public event Action onEnd;
    
    [SerializeField] VfxSequenceUpdateType sequenceUpdateType = VfxSequenceUpdateType.Update;
    
    Dictionary<int, List<VfxSequence>> sequences = new();
    List<VfxSequence> playingSequences = new();

    VfxSequenceStatusType statusType;
    int maxOrder;
    int progressOrder;

    public void StartSequence()
    {
        progressOrder = 0;
        StartSequencesByOrder(progressOrder);
    }
    
    // TODO : Disable - Enable 설정 시 재생상태 결정 필요
    void OnEnable()
    {
        statusType = VfxSequenceStatusType.None;
    }
    
    void Awake()
    {
        var sequenceComponents = GetComponents<VfxSequence>();
        foreach (var sequence in sequenceComponents)
        {
            AddSequence(sequence);
            CompareProcessOrder(sequence);
        }
    }

    void AddSequence(VfxSequence vfxSequence)
    {
        if (!sequences.TryGetValue(vfxSequence.ProcessOrder, out var seqs))
        {
            seqs = new();
            sequences.Add(vfxSequence.ProcessOrder, seqs);
        }
        
        seqs.Add(vfxSequence);
    }

    void CompareProcessOrder(VfxSequence vfxSequence)
    {
        if (vfxSequence.ProcessOrder >= maxOrder)
        {
            maxOrder = vfxSequence.ProcessOrder;
        }
    }

    void Update()
    {
        if (statusType != VfxSequenceStatusType.Playing)
        {
            return;
        }

        foreach (var seq in playingSequences)
        {
            seq.UpdateSequence(GetUpdateTime());
        }

        if (playingSequences.All(seq => seq.StatusType == VfxSequenceStatusType.Finish))
        {
            onFinish?.Invoke(progressOrder, maxOrder);
            statusType = VfxSequenceStatusType.Finish;
            
            progressOrder++;
            if (progressOrder <= maxOrder)
            {
                StartSequencesByOrder(progressOrder);
            }
        }
    }
    
    float GetUpdateTime()
    {
        return sequenceUpdateType switch
        {
            VfxSequenceUpdateType.Update => Time.deltaTime,
            VfxSequenceUpdateType.FixedUpdate => Time.fixedDeltaTime,
            _ => Time.deltaTime
        };
    }

    void StartSequencesByOrder(int order)
    {
        statusType = VfxSequenceStatusType.Playing;
        playingSequences = sequences.FirstOrDefault(kv => kv.Key == order).Value;

        foreach (var seq in playingSequences)
        {
            seq.StartSequence();
        }

        onStart?.Invoke(order);
    }
}
