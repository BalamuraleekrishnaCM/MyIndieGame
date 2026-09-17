using System;

namespace MyIndieGame.Core
{
    /// <summary>
    /// Shared, game-agnostic lifecycle and score state for one mini-game run.
    /// </summary>
    public sealed class MiniGameSession
    {
        readonly MiniGameDefinition definition;
        readonly int gameIndex;
        bool paused;

        public MiniGameSession(MiniGameDefinition definition) : this(definition, -1) { }

        // Kept for compatibility with existing game adapters that carry a catalog index.
        public MiniGameSession(MiniGameDefinition definition, int gameIndex)
        {
            this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
            this.gameIndex = gameIndex;
        }

        public string GameId => definition.Id;
        public int GameIndex => gameIndex;
        public int Score { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsPaused => paused;

        public event Action<int> ScoreChanged;
        public event Action<int> Completed;

        public void Start()
        {
            Score = 0;
            paused = false;
            IsActive = true;
            ScoreChanged?.Invoke(Score);
        }

        public void Pause()
        {
            if (!IsActive) return;
            paused = true;
        }

        public void Resume()
        {
            if (!IsActive) return;
            paused = false;
        }

        public void AddScore(int amount)
        {
            if (!IsActive || paused || amount <= 0) return;
            Score = SafeAdd(Score, amount);
            ScoreChanged?.Invoke(Score);
        }

        public void End()
        {
            if (!IsActive) return;
            IsActive = false;
            paused = false;
        }

        public void Complete()
        {
            if (!IsActive) return;
            int finalScore = Score;
            IsActive = false;
            paused = false;

            // Route all completed runs through the same persistence/progression pipeline.
            SessionProgress.Begin(GameId);
            SessionProgress.AddScore(finalScore);
            SessionProgress.Finish();
            Completed?.Invoke(finalScore);
        }

        static int SafeAdd(int current, int amount)
        {
            return current > int.MaxValue - amount ? int.MaxValue : current + amount;
        }
    }
}
