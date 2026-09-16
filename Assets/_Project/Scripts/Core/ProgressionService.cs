using UnityEngine;

namespace MyIndieGame.Core
{
    public static class ProgressionService
    {
        const string DifficultyPrefix = "mig.difficulty.";

        public static int GetDifficulty(string gameId)
        {
            return Mathf.Clamp(PlayerPrefs.GetInt(DifficultyPrefix + gameId, 1), 1, 5);
        }

        public static int GetGamesPlayedDifficulty(string gameId)
        {
            int best = BestScoreStore.Get(gameId);
            return Mathf.Clamp(1 + best / 25, 1, 5);
        }

        public static int GetEffectiveDifficulty(string gameId)
        {
            return Mathf.Max(GetDifficulty(gameId), GetGamesPlayedDifficulty(gameId));
        }

        public static float GetTimeLimit(string gameId, float baseTime)
        {
            int difficulty = GetEffectiveDifficulty(gameId);
            return Mathf.Max(15f, baseTime - (difficulty - 1) * 2f);
        }

        public static int CalculateReward(string gameId, int score)
        {
            int difficulty = GetEffectiveDifficulty(gameId);
            return Mathf.Max(1, 2 + score / 5 + difficulty - 1);
        }
    }
}
