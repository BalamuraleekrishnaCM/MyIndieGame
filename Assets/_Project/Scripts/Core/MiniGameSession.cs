using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    /// <summary>
    /// Coordinates one mini-game session with the shared navigation and local progress services.
    /// It does not grant economy or leaderboard rewards; those remain server-authoritative.
    /// </summary>
    public sealed class MiniGameSession
    {
        public GameDefinition Definition { get; }
        public bool IsActive { get; private set; }
        public int Score { get; private set; }

        public event Action<int> ScoreChanged;
        public event Action<int> Completed;

        public MiniGameSession(GameDefinition definition)
        {
            Definition = definition;
        }

        public bool Start()
        {
            if (Definition == null || IsActive) return false;
            Score = 0;
            IsActive = true;
            GameNavigationService.OpenGame(IndexOf(Definition.Id));
            ScoreChanged?.Invoke(Score);
            return true;
        }

        public void SetScore(int score)
        {
            if (!IsActive) return;
            Score = Mathf.Max(0, score);
            ScoreChanged?.Invoke(Score);
        }

        public void AddScore(int delta)
        {
            SetScore(Score + delta);
        }

        public void Complete()
        {
            if (!IsActive) return;
            IsActive = false;
            GameNavigationService.ShowResults(Score);
            Completed?.Invoke(Score);
        }

        public void Pause() { }
        public void Resume() { }

        public void End()
        {
            if (!IsActive) return;
            IsActive = false;
        }

        static int IndexOf(string id)
        {
            for (int i = 0; i < GameCatalog.All.Count; i++)
                if (GameCatalog.All[i].Id == id) return i;
            return -1;
        }
    }
}
