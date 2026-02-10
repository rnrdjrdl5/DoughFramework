using UnityEngine;

public interface IInputAbility
{
    void SetWorldReceiver(IWorldInputReceiver receiver);
    void SetWorldCamera(Camera cam);
    void Enable(bool enabled);
}
