using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;


public class CountingNumberVfx : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] float duration;
    
    Tween playedTween;
    int progressValue;

    public void CountingAnimation(int start, int end)
    {
        CountingAnimation(start, end, duration);
    }
    
    public void CountingAnimation(int start, int end, float duration)
    {
        if (playedTween != null)
        {
            if (playedTween.IsActive() && playedTween.IsPlaying())
            {
                playedTween.Kill();
            }
            
            playedTween = null;
        }

        progressValue = start;
        text.text = $"{progressValue}";
        
        playedTween = DOTween.To(
            () => progressValue,
            x =>
            {
                progressValue = x;
                text.text = x.ToString();
            }, end, duration);
    }

    // 필요 시 구현
    public async UniTask CountingAnimationAsync(int target)
    {
        
    }
}
