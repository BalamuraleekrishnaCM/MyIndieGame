using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class AudioFeedback : MonoBehaviour
    {
        public static AudioFeedback Instance { get; private set; }

        [SerializeField] AudioClip successClip;
        [SerializeField] AudioClip failClip;
        [SerializeField] AudioClip clickClip;
        [SerializeField, Range(0f, 1f)] float volume = 0.7f;

        AudioSource source;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }

        public void Click() => Play(clickClip);
        public void Success() => Play(successClip);
        public void Fail() => Play(failClip);

        void Play(AudioClip clip)
        {
            if (clip != null) source.PlayOneShot(clip, volume);
        }
    }
}
