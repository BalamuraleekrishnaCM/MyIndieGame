using System;
using MyIndieGame.Core;
using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class DodgeLineGame : MonoBehaviour, IMiniGame
    {
        private const string StableGameId = "dodge-line";
        private const int DefaultIndex = 3;
        private MiniGameSession _session;
        private bool _initialized;
        public string GameId => StableGameId;
        public bool IsRunning => _session != null && _session.IsActive;
        public int Score => _session?.Score ?? 0;
        public event Action<int> ScoreChanged;
        public event Action<int> Completed;
        private void Awake() => Initialize();
        public void Initialize()
        {
            if (_initialized) return;
            if (!GameCatalog.TryGet(GameId, out var definition)) throw new InvalidOperationException($"Missing game definition: {GameId}");
            _session = new MiniGameSession(definition, DefaultIndex);
            _session.ScoreChanged += value => ScoreChanged?.Invoke(value);
            _session.Completed += value => Completed?.Invoke(value);
            _initialized = true;
        }
        public void StartGame() { Initialize(); _session.Start(); }
        public void PauseGame() => _session?.Pause();
        public void ResumeGame() => _session?.Resume();
        public void EndGame() => _session?.End();
        public void RecordDodge(int points = 1)
        {
            if (!IsRunning || points <= 0) return;
            _session.AddScore(points);
        }
        public void CompleteGame() { if (IsRunning) _session.Complete(); }
    }
}
