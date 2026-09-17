using System;
using UnityEngine;
using MyIndieGame.Core;

namespace MyIndieGame.MiniGames
{
    public abstract class ProductionMiniGameBase : MonoBehaviour, IMiniGame
    {
        [SerializeField] float durationSeconds = 30f;

        MiniGameSession session;
        float remaining;
        bool paused;

        public abstract string GameId { get; }
        public bool IsRunning => session != null && session.IsActive && !paused;
        public int Score => session?.Score ?? 0;

        public event Action<int> ScoreChanged;
        public event Action<int> Completed;

        protected virtual float DurationSeconds => Mathf.Max(1f, durationSeconds);

        protected virtual void Awake()
        {
            if (!GameCatalog.TryGet(GameId, out var definition))
                throw new InvalidOperationException($"Unknown game id: {GameId}");
            session = new MiniGameSession(definition);
            session.ScoreChanged += HandleScoreChanged;
            session.Completed += HandleCompleted;
        }

        protected virtual void OnDestroy()
        {
            if (session == null) return;
            session.ScoreChanged -= HandleScoreChanged;
            session.Completed -= HandleCompleted;
        }

        public virtual void StartGame()
        {
            if (session == null || session.IsActive) return;
            ResetGame();
            remaining = DurationSeconds;
            paused = false;
            session.Start();
            OnGameStarted();
        }

        public virtual void PauseGame()
        {
            if (!IsRunning) return;
            paused = true;
            session.Pause();
            OnGamePaused();
        }

        public virtual void ResumeGame()
        {
            if (session == null || !session.IsActive || !paused) return;
            paused = false;
            session.Resume();
            OnGameResumed();
        }

        public virtual void EndGame()
        {
            if (session == null || !session.IsActive) return;
            session.End();
            OnGameEnded();
        }

        protected void AddScore(int amount)
        {
            if (!IsRunning || amount <= 0) return;
            session.AddScore(amount);
        }

        protected void CompleteGame()
        {
            if (session == null || !session.IsActive) return;
            session.Complete();
        }

        protected virtual void Update()
        {
            if (!IsRunning) return;
            remaining -= Time.unscaledDeltaTime;
            if (remaining <= 0f) CompleteGame();
            Tick(remaining);
        }

        protected virtual void ResetGame() { }
        protected virtual void OnGameStarted() { }
        protected virtual void OnGamePaused() { }
        protected virtual void OnGameResumed() { }
        protected virtual void OnGameEnded() { }
        protected virtual void Tick(float remainingSeconds) { }

        void HandleScoreChanged(int value) => ScoreChanged?.Invoke(value);
        void HandleCompleted(int value)
        {
            Completed?.Invoke(value);
            OnGameCompleted(value);
        }

        protected virtual void OnGameCompleted(int finalScore) { }
    }
}
