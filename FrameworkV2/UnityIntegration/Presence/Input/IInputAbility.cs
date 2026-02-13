using UnityEngine;

public interface IInputAbility
{
    void SetWorldReceiver(IWorldInputReceiver receiver);
    void SetWorldCamera(Camera cam);
    void SetUIAbility(IUIAbility ability);
    void Enable(bool enabled);
}
