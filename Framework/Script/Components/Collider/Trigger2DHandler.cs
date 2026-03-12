using System;
using UnityEngine;

public class Trigger2DHandler : MonoBehaviour
{
    public System.Action<Collider2D> OnTriggerEnter;
    public System.Action<Collider2D> OnTriggerExit;

    void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnter?.Invoke(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        OnTriggerExit?.Invoke(other);
    }
}
