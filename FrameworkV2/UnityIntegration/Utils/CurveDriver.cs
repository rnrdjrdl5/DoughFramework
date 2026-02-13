using System;
using UnityEngine;

namespace App
{
    public class CurveDriver : MonoBehaviour
    {
        public bool IsPlaying => isPlaying;
        public float Duration => duration;
        public float CurrentTime => time;
        public float NormalizedTime => duration > 0f ? Mathf.Clamp01(time / duration) : 1f;

        public Action<float, float> OnUpdate; // (normalizedTime, evaluatedValue)
        public Action OnComplete;
        
        [Header("Curve Settings")] [SerializeField] AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] float duration = 1f;
        [SerializeField] bool ignoreTimeScale = false;
        [SerializeField] bool autoPlay = false;

        float time;
        bool isPlaying;

        void OnEnable()
        {
            if (autoPlay)
            {
                Play();
            }
        }

        void OnDisable()
        {
            Stop();
        }

        public void Play()
        {
            time = 0f;
            isPlaying = true;
        }

        public void Play(float customDuration)
        {
            duration = Mathf.Max(0.0001f, customDuration);
            Play();
        }

        public void Stop()
        {
            isPlaying = false;
        }

        public void Complete()
        {
            time = duration;
            var value = Evaluate(time);
            OnUpdate?.Invoke(1f, value);
            isPlaying = false;
            OnComplete?.Invoke();
        }
        
        /// <summary>
        /// 특정 시간(초)에 대한 Curve 값 계산
        /// </summary>
        public float Simulate(float targetTime)
        {
            return Evaluate(targetTime);
        }

        /// <summary>
        /// 0~1 정규화 시간에 대한 Curve 값 계산
        /// </summary>
        public float EvaluateNormalized(float normalizedTime)
        {
            return curve.Evaluate(Mathf.Clamp01(normalizedTime));
        }

        float Evaluate(float targetTime)
        {
            if (duration <= 0f)
                return curve.Evaluate(1f);

            var t = Mathf.Clamp01(targetTime / duration);
            return curve.Evaluate(t);
        }

        void Update()
        {
            if (!isPlaying)
            {
                return;
            }

            var delta = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            time += delta;

            var normalized = NormalizedTime;
            var value = curve.Evaluate(normalized);

            OnUpdate?.Invoke(normalized, value);

            if (time >= duration)
            {
                isPlaying = false;
                OnComplete?.Invoke();
            }
        }
    }

}