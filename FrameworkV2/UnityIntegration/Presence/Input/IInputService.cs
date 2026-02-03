using UnityEngine;

public interface IInputService : IService
{
    void SetWorldReceiver(IWorldInputReceiver receiver);
    void SetWorldCamera(Camera cam);
    void Enable(bool enabled);
}

