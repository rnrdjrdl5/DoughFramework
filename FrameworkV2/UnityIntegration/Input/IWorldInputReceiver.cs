using UnityEngine;

public interface IWorldInputReceiver
{
    void OnWorldPointerDown(Vector2 screenPos, Ray worldRay);
    void OnEsc();
}

