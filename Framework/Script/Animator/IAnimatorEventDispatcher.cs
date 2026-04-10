public interface IAnimatorEventDispatcher
{
    void Dispatch(AnimationEventReceiver sender, string eventKey);
}
