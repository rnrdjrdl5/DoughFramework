using DG.Tweening;
using UnityEngine;

public class ShakeObjectVfx : MonoBehaviour
{
    Tween playedTween;
    
    public void ShakeObject(float duration)
    {
        if (playedTween != null)
        {
            if (playedTween.IsActive() && playedTween.IsPlaying())
            {
                playedTween.Kill();
            }
            
            playedTween = null;
        }
        
        playedTween = transform.DOShakePosition(duration);
    }
}
