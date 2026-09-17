using System;
using MyIndieGame.Core;
using UnityEngine;

namespace MyIndieGame.MiniGames
{
    /// <summary>
    /// Production architecture boundary for Tap Rush.
    /// Gameplay UI/input can drive AddTap while session lifecycle remains centralized.
    /// </summary>
    public sealed class TapRushGame : MonoBehaviour, IMiniGame
    {
        private const string StableGameId = "tap-rush";
        private const int DefaultIndex = 0;

        private MiniGameSession _session;
        private bool _initialized;

        public string GameId => StableGameId;
        public bool IsRunning => _session != null && _session.IsActive;
        public int Score => _session?.Score ?? 0;

        public event Action<int> ScoreChanged;
        public event Action<int> Completed;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (_initialized)
                return;

            if (!GameCatalog.TryGet(GameId, out var definition))
                throw new InvalidOperationException($"Missing game definition: {GameId}");

            _session = new MiniGameSession(definition, DefaultIndex);
            _session.ScoreChanged += HandleScoreChanged;
            _session.Completed += HandleCompleted;
            _initialized = true;
        }

        public void StartGame()
        {
            Initialize();
            _session.Start();
        }

        public void PauseGame()
        {
            _session?.Pause();
        }

        public void ResumeGame()
        {
            _session?.Resume();
        }

        public void EndGame()
        {
            _session?.End();
        }

        /// <summary>
        /// Called by the Tap Rush input layer for each valid target tap.
        /// </summary>
        public void AddTap(int points = 1)
        {
            if (!IsRunning || points <= 0)
                return;

            _session.AddScore(points);
        }

        /// <summary>
        /// Completes the current run and routes the result through shared navigation/progress.
        /// </summary>
        public void CompleteGame()
        {
            if (IsRunning)
                _session.Complete();
        }

        private void HandleScoreChanged(int value)
        {
            ScoreChanged?.Invoke(value);
        }

        private void HandleCompleted(int value)
        {
            Completed?.Invoke(value);
        }

        private void OnDestroy()
        {
            if (_session == null)
                return;

            _session.ScoreChanged -= HandleScoreChanged;
            _session.Completed -= HandleCompleted;
        }
    }
}
