using UnityEngine;

namespace MyIndieGame.Core
{
    public static class ReleaseReadiness
    {
        public const string ReleaseVersion = "1.0.0";

        public static bool IsMobilePlatform()
        {
            return Application.platform == RuntimePlatform.Android ||
                   Application.platform == RuntimePlatform.IPhonePlayer;
        }

        public static void PrepareForRelease()
        {
            Time.timeScale = 1f;
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
        }
    }
}
