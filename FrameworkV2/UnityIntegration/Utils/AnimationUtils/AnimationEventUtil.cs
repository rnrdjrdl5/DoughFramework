using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App
{
    /// <summary>
    /// Animation Event 대기를 위한 Animator 확장 메서드
    /// </summary>
    public static class AnimationEventUtil
    {
        /// <summary>
        /// 특정 Animation Event가 발생할 때까지 대기
        /// </summary>
        /// <param name="animator">대상 Animator</param>
        /// <param name="eventName">대기할 이벤트 이름. null이면 다음 모든 Animation Event를 수신</param>
        /// <param name="cancellationToken">취소 토큰</param>
        /// <returns>발생한 이벤트 이름</returns>
        /// <example>
        /// <code>
        /// animator.SetTrigger("Attack");
        /// string eventName = await animator.WaitForAnimationEventAsync("AttackHit");
        /// ApplyDamage();
        /// </code>
        /// </example>
        public static async UniTask<string> WaitForAnimationEventAsync(
            this Animator animator,
            string eventName = null,
            CancellationToken cancellationToken = default)
        {
            var awaiter = GetOrAddAwaiter(animator);
            return await awaiter.WaitForEventAsync(eventName, cancellationToken);
        }

        /// <summary>
        /// 다음 Animation Event가 발생할 때까지 대기 (이벤트 이름 무관)
        /// </summary>
        /// <param name="animator">대상 Animator</param>
        /// <param name="cancellationToken">취소 토큰</param>
        /// <returns>발생한 이벤트 이름</returns>
        /// <example>
        /// <code>
        /// animator.SetTrigger("Upgrade");
        /// string eventName = await animator.WaitForNextAnimationEventAsync();
        /// Debug.Log($"Event triggered: {eventName}");
        /// </code>
        /// </example>
        public static async UniTask<string> WaitForNextAnimationEventAsync(
            this Animator animator,
            CancellationToken cancellationToken = default)
        {
            return await WaitForAnimationEventAsync(animator, null, cancellationToken);
        }

        /// <summary>
        /// Animator에서 AnimationEventAwaiter 컴포넌트를 가져오거나 추가
        /// </summary>
        private static AnimationEventAwaiter GetOrAddAwaiter(Animator animator)
        {
            if (animator == null)
                throw new ArgumentNullException(nameof(animator));

            var awaiter = animator.GetComponent<AnimationEventAwaiter>();
            if (awaiter == null)
            {
                awaiter = animator.gameObject.AddComponent<AnimationEventAwaiter>();
            }

            return awaiter;
        }
    }
}
