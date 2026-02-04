using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App
{
    /// <summary>
    /// Animation Event를 수신하여 UniTask를 완료시키는 컴포넌트
    /// Unity Animation Event에서 OnAnimationEventTriggered 메서드를 호출하여 사용
    /// </summary>
    public class AnimationEventAwaiter : MonoBehaviour
    {
        private UniTaskCompletionSource<string> completionSource;
        private string targetEventName;
        private bool isWaiting;

        /// <summary>
        /// Animation Event에서 호출될 메서드
        /// Unity Editor의 Animation Event Function에 이 메서드 이름을 설정
        /// </summary>
        /// <param name="eventName">Animation Event의 String Parameter</param>
        public void OnAnimationEventTriggered(string eventName)
        {
            if (!isWaiting) return;

            // targetEventName이 null이면 모든 이벤트 수신, 아니면 일치하는 이벤트만 수신
            if (string.IsNullOrEmpty(targetEventName) || targetEventName == eventName)
            {
                completionSource?.TrySetResult(eventName);
                ResetState();
            }
        }

        /// <summary>
        /// Animation Event가 발생할 때까지 대기
        /// </summary>
        /// <param name="eventName">대기할 이벤트 이름. null이면 다음 모든 Animation Event를 수신</param>
        /// <param name="cancellationToken">취소 토큰</param>
        /// <returns>발생한 이벤트 이름</returns>
        public UniTask<string> WaitForEventAsync(string eventName = null, CancellationToken cancellationToken = default)
        {
            if (isWaiting)
            {
                Debug.LogWarning($"[AnimationEventAwaiter] Already waiting for an animation event on {gameObject.name}");
                return UniTask.FromResult<string>(null);
            }

            targetEventName = eventName;
            isWaiting = true;
            completionSource = new UniTaskCompletionSource<string>();

            // CancellationToken 처리
            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() =>
                {
                    if (isWaiting)
                    {
                        completionSource?.TrySetCanceled();
                        ResetState();
                    }
                });
            }

            return completionSource.Task;
        }

        private void ResetState()
        {
            isWaiting = false;
            targetEventName = null;
            completionSource = null;
        }

        void OnDestroy()
        {
            // GameObject가 파괴될 때 대기 중인 Task 취소
            if (isWaiting)
            {
                completionSource?.TrySetCanceled();
                ResetState();
            }
        }
    }
}
