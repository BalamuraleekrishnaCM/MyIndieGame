using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class MobilePerformance : MonoBehaviour
    {
        [SerializeField] int targetFrameRate = 60;

        void Awake()
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = 0;
            Input.multiTouchEnabled = true;
        }
    }
}
