using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class SceneBootstrap : MonoBehaviour
    {
        static bool initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            if (initialized) return;
            initialized = true;

            GameObject root = new GameObject("AppSystems");
            Object.DontDestroyOnLoad(root);
            root.AddComponent<GameSession>();
            root.AddComponent<GameFeel>();
            root.AddComponent<AudioFeedback>();
            root.AddComponent<VFXFeedback>();
        }
    }
}
