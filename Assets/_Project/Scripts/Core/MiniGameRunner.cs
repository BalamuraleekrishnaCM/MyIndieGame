using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class MiniGameRunner : MonoBehaviour
    {
        [SerializeField] string gameId = "tap_rush";
        [SerializeField] float duration = 30f;
        public float TimeRemaining { get; private set; }
        public bool IsRunning { get; private set; }

        void Start() { Begin(gameId); }

        public void Begin(string id)
        {
            gameId = id;
            TimeRemaining = duration;
            IsRunning = true;
            SessionProgress.Begin(id);
        }

        public void AddScore(int amount)
        {
            if (!IsRunning) return;
            SessionProgress.AddScore(amount);
        }

        public void End()
        {
            if (!IsRunning) return;
            IsRunning = false;
            SessionProgress.Finish();
        }

        void Update()
        {
            if (!IsRunning) return;
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining <= 0f) { TimeRemaining = 0f; End(); }
        }
    }
}
