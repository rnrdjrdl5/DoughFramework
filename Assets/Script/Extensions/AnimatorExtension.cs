using UnityEngine;

public static class AnimatorExtension
{
    public static bool TryGetAnimLength(this Animator animator, string animationName, out float length)
    {
        length = 0f;
        
        if (animator == null || animator.runtimeAnimatorController == null)
            return false;
            
        var clips = animator.runtimeAnimatorController.animationClips;
        
        foreach (var clip in clips)
        {
            if (clip.name == animationName)
            {
                length = clip.length;
                
                return true;
            }
        }
        
        return false;
    }

    public static void PlayAnimByState(this Animator animator, int state, bool isRefresh = false)
    {
        animator.SetInteger(AnimatorParameters.AnimationState, state);
        
        if (isRefresh)
        {
            animator.SetTrigger(AnimatorParameters.Refresh);
        }
    }
}