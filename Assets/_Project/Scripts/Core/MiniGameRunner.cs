using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class MiniGameRunner : MonoBehaviour
    {
        [SerializeField] string gameId = "tap_rush";
        [SerializeField, Min(0.1f)] float duration = 30f;
        public float TimeRemaining { get; private set; }
        public bool IsRunning { get; private set; }

        void Start() { Begin(gameId); }

        public void Begin(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                IsRunning = false;
                TimeRemaining = 0f;
                SessionProgress.Reset();
                return;
            }

            gameId = id.Trim();
            TimeRemaining = Mathf.Max(0.1f, duration);
            IsRunning = true;
            SessionProgress.Begin(gameId);
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
            TimeRemaining = 0f;
            SessionProgress.Finish();
        }

        void OnDisable()
        {
            // Prevent a disabled/destroyed runner from leaving a stale active
            // SessionProgress session that another runner could finish later.
            if (IsRunning)
            {
                IsRunning = false;
                SessionProgress.Reset();
            }
        }

        void Update()
        {
            if (!IsRunning) return;
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining <= 0f) End();
        }
    }
}
