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
            CurrentScore = SafeAdd(CurrentScore, amount);
            if (CurrentScore < 0) CurrentScore = 0;
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

            PlayerStatsService.RecordGame(finalScore, false);
            MissionService.RecordGame(finalScore);
            AnalyticsService.RecordGameCompleted(gameId, finalScore, LastReward);
            AchievementService.Evaluate(gameId, finalScore, GameSession.Instance != null ? GameSession.Instance.Coins : 0, PlayerStatsService.GamesPlayed);

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

        static int SafeAdd(int current, int amount)
        {
            if (amount > 0 && current > int.MaxValue - amount) return int.MaxValue;
            if (amount < 0 && current < int.MinValue - amount) return int.MinValue;
            return current + amount;
        }
    }
}
