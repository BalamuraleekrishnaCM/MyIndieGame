using UnityEngine;

namespace MyIndieGame.Core
{
    public static class FeedbackService
    {
        public static void Success(Transform target = null)
        {
            if (AppSettings.SoundEnabled && AudioFeedback.Instance != null) AudioFeedback.Instance.Success();
            if (AppSettings.HapticsEnabled) HapticFeedback.Light();
            if (VFXFeedback.Instance != null) VFXFeedback.Instance.Hit(target);
        }

        public static void Fail(Transform target = null)
        {
            if (AppSettings.SoundEnabled && AudioFeedback.Instance != null) AudioFeedback.Instance.Fail();
            if (AppSettings.HapticsEnabled) HapticFeedback.Light();
            if (VFXFeedback.Instance != null) VFXFeedback.Instance.Hit(target);
        }

        public static void Click()
        {
            if (AppSettings.SoundEnabled && AudioFeedback.Instance != null) AudioFeedback.Instance.Click();
        }
    }
}
