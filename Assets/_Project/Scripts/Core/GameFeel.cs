using System.Collections;
using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class GameFeel : MonoBehaviour
    {
        public static GameFeel Instance { get; private set; }
        [SerializeField] float defaultShake = 0.08f;
        Coroutine shakeRoutine;

        void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

        public void Punch(Transform target, float scale = 1.08f, float duration = 0.08f)
        {
            if (target == null) return;
            StartCoroutine(PunchRoutine(target, scale, duration));
        }

        IEnumerator PunchRoutine(Transform target, float scale, float duration)
        {
            Vector3 original = target.localScale;
            target.localScale = original * scale;
            yield return new WaitForSecondsRealtime(duration);
            if (target) target.localScale = original;
        }

        public void HapticLight()
        {
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }

        public void ScreenShake(Camera camera, float amount = -1f, float duration = 0.12f)
        {
            if (!camera) return;
            if (amount < 0) amount = defaultShake;
            if (shakeRoutine != null) StopCoroutine(shakeRoutine);
            shakeRoutine = StartCoroutine(ShakeRoutine(camera.transform, amount, duration));
        }

        IEnumerator ShakeRoutine(Transform target, float amount, float duration)
        {
            Vector3 origin = target.localPosition;
            float t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                target.localPosition = origin + Random.insideUnitSphere * amount;
                yield return null;
            }
            target.localPosition = origin;
            shakeRoutine = null;
        }
    }
}
