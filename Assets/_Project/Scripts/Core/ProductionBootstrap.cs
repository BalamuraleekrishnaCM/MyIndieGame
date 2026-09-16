using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class ProductionBootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void EnsureRuntimeSystems()
        {
            if (Object.FindFirstObjectByType<MobilePerformance>() != null) return;
            new GameObject("MobilePerformance").AddComponent<MobilePerformance>();
        }
    }
}
