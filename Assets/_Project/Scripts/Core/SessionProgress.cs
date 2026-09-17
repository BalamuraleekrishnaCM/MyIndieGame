using UnityEngine;

namespace MyIndieGame.Core
{
    public static class SessionProgress
    {
        public static int CurrentScore { get; private set; }
        public static string CurrentGameId { get; private set; }
        public static int LastReward { get; private set; }
        public static bool IsActive => !string.IsNullOrEmpty(CurrentGameId);

        public static void Begin(string gameId)
        {
            if (string.IsNullOrWhiteSpace(gameId))
            {
                Reset();
                return;
            }

            CurrentGameId = gameId.Trim();
            CurrentScore = 0;
            LastReward = 0;
        }

        public static void AddScore(int amount)
        {
            if (!IsActive || amount == 0) return;
            CurrentScore = Mathf.Max(0, CurrentScore + amount);
        }

        public static bool Finish()
        {
            if (!IsActive) return false;

            string gameId = CurrentGameId;
            int finalScore = CurrentScore;
            bool best = BestScoreStore.Submit(gameId, finalScore);
            LastReward = ProgressionService.CalculateReward(gameId, finalScore);

            if (GameSession.Instance != null)
            {
                GameSession.Instance.AddCoins(LastReward);
                GameSession.Instance.RecordGamePlayed();
            }

            CurrentGameId = null;
            CurrentScore = 0;
            return best;
        }

        public static void Reset()
        {
            CurrentGameId = null;
            CurrentScore = 0;
            LastReward = 0;
        }
    }
}
