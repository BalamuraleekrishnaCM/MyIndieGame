using UnityEngine;

namespace MyIndieGame.Core
{
    public static class AppSettings
    {
        const string SoundKey = "mig.settings.sound";
        const string HapticsKey = "mig.settings.haptics";

        public static bool SoundEnabled
        {
            get => PlayerPrefs.GetInt(SoundKey, 1) == 1;
            set { PlayerPrefs.SetInt(SoundKey, value ? 1 : 0); PlayerPrefs.Save(); }
        }

        public static bool HapticsEnabled
        {
            get => PlayerPrefs.GetInt(HapticsKey, 1) == 1;
            set { PlayerPrefs.SetInt(HapticsKey, value ? 1 : 0); PlayerPrefs.Save(); }
        }
    }
}
