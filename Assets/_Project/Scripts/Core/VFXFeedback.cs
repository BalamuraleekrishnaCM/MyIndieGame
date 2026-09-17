using System.Collections;
using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class VFXFeedback : MonoBehaviour
    {
        public static VFXFeedback Instance { get; private set; }

        [SerializeField] float flashDuration = 0.08f;
        [SerializeField] float punchScale = 1.08f;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Hit(Transform target)
        {
            if (target != null) StartCoroutine(Punch(target));
        }

        public void ScreenFlash(CanvasGroup overlay, float alpha = 0.16f)
        {
            if (overlay != null) StartCoroutine(Flash(overlay, alpha));
        }

        IEnumerator Punch(Transform target)
        {
            Vector3 original = target.localScale;
            target.localScale = original * punchScale;
            yield return new WaitForSecondsRealtime(flashDuration);
            if (target != null) target.localScale = original;
        }

        IEnumerator Flash(CanvasGroup overlay, float alpha)
        {
            overlay.alpha = Mathf.Clamp01(alpha);
            yield return new WaitForSecondsRealtime(flashDuration);
            if (overlay != null) overlay.alpha = 0f;
        }
    }
}
