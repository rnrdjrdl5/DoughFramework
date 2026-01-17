using UnityEngine;

public static class AnimatorParameters
{
    static int GetHashCode(string parameter) => Animator.StringToHash(parameter);
    public static int AnimationState = GetHashCode(nameof(AnimationState));
    public static int Refresh = GetHashCode(nameof(Refresh));
}