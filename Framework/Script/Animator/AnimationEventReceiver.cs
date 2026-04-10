using UnityEngine;

[DisallowMultipleComponent]
public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] Animator animator;

    public Animator Animator => animator;
    public IAnimatorEventDispatcher Dispatcher => dispatcher;

    IAnimatorEventDispatcher dispatcher;

    public void SetAnimator(Animator value)
    {
        animator = value;
    }

    public void SetDispatcher(IAnimatorEventDispatcher value)
    {
        dispatcher = value;
    }

    public void ClearDispatcher()
    {
        dispatcher = null;
    }

    public void OnAnimationEvent(string eventKey)
    {
        if (string.IsNullOrWhiteSpace(eventKey))
        {
            return;
        }

        dispatcher?.Dispatch(this, eventKey);
    }
}
