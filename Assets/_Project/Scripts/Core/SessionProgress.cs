using UnityEngine;

namespace MyIndieGame.Core
{
    public static class SessionProgress
    {
        public static int CurrentScore { get; private set; }
        public static string CurrentGameId { get; private set; }

        public static void Begin(string gameId) { CurrentGameId = gameId; CurrentScore = 0; }
        public static void AddScore(int amount) { CurrentScore = Mathf.Max(0, CurrentScore + amount); }

        public static bool Finish()
        {
            if (string.IsNullOrEmpty(CurrentGameId)) return false;
            bool best = BestScoreStore.Submit(CurrentGameId, CurrentScore);
            if (GameSession.Instance != null) GameSession.Instance.RecordGamePlayed();
            return best;
        }
    }
}
