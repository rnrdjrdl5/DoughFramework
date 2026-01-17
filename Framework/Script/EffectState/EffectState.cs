using Cysharp.Threading.Tasks;
using UnityEngine;

public class EffectState<TSharedData> : State
{
    EffectStateRefresher<TSharedData> startRefresher;
    EffectStateRefresher<TSharedData> endRefresher;
    TSharedData sharedData;

    public override async UniTask OnEnterStateAsync()
    {
        await base.OnEnterStateAsync();
        
        startRefresher?.Refresh();
    }

    public override void OnExitState()
    {
        endRefresher?.Refresh();

        UninitializeRefresher();
        
        base.OnExitState();
    }

    public void SetStateData(TSharedData sharedData)
    {
        this.sharedData = sharedData;
    }

    public void SetStartRefresher(EffectStateRefresher<TSharedData> startRefresher)
    {
        this.startRefresher = startRefresher;
    }

    public void SetEndRefresher(EffectStateRefresher<TSharedData> endRefresher)
    {
        this.endRefresher = endRefresher;
    }
    
    void UninitializeRefresher()
    {
        startRefresher = null;
        endRefresher = null;
    }
}

public abstract class EffectStateRefresher<TSharedData> : IRefresh
{
    TSharedData sharedData;
    EffectStateData effectStateData;
    
    public EffectStateRefresher(TSharedData sharedData)
    {
        this.sharedData = sharedData;
    }

    public void SetEffectStateData(EffectStateData effectStateData)
    {
        this.effectStateData = effectStateData;
    }
    
    public abstract void Refresh();
}

public abstract class EffectStateData
{
    
}