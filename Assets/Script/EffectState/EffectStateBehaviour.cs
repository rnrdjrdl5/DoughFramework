using System.Threading;
using Cysharp.Threading.Tasks;

public class EffectStateBehaviour<TSharedData> : StateBehaviour
{
    public EffectState<TSharedData> ActivateState<T>(EffectStateRefresher<TSharedData> originRefresher, EffectStateRefresher<TSharedData> endRefresher) where T : State
    {
        var state = GetState<T>();
        if (state == null || state is not EffectState<TSharedData> targetState)
        {
            return null;
        }

        targetState.SetStartRefresher(originRefresher);
        targetState.SetEndRefresher(endRefresher);

        ActivateState(targetState);

        return targetState;
    }

    public override State ActivateState<StateType>()
    {
        var state = GetState<StateType>();
        if (state == null || state is not EffectState<TSharedData> effectState)
        {
            return state;
        }

        ActivateState(effectState);
        
        return effectState;
    }

    public async UniTask WaitState<TEffectState>(CancellationToken cancellationToken = default) where TEffectState : State
    {
        await UniTask.WaitUntil(IsActivateState<TEffectState>, cancellationToken: cancellationToken);
    }
}
