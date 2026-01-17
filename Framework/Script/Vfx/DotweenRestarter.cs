using System;
using DG.Tweening;
using UnityEngine;

public class DotweenRestarter : MonoBehaviour
{
    DOTweenAnimation doTweenAnimation;
    private void Awake()
    {
        doTweenAnimation = GetComponent<DOTweenAnimation>();
        // NOTE : 현재는 수동으로 꺼야 함.
        //doTweenAnimation.autoKill = false;
    }

    private void OnEnable()
    {
        doTweenAnimation.DORestart();
    }
}
