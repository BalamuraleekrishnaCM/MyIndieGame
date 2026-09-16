using UnityEngine;

namespace MyIndieGame.Core
{
    public static class HapticFeedback
    {
        public static void Light()
        {
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }
    }
}
