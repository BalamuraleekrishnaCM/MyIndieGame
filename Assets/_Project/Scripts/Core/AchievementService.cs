using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyIndieGame.Core
{
    public static class AchievementService
    {
        public static readonly string[] Ids = { "first_game", "score_100", "play_10", "earn_100_coins" };
        public static event Action Changed;

        public static bool IsUnlocked(string id) => PlayerPrefs.GetInt("mig.achievement." + id, 0) == 1;

        public static bool Unlock(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || IsUnlocked(id)) return false;
            PlayerPrefs.SetInt("mig.achievement." + id, 1);
            PlayerPrefs.Save();
            AnalyticsService.RecordRewardClaimed(id);
            Changed?.Invoke();
            return true;
        }

        public static void Evaluate(string gameId, int score, int coins, int gamesPlayed)
        {
            if (gamesPlayed >= 1) Unlock("first_game");
            if (score >= 100) Unlock("score_100");
            if (gamesPlayed >= 10) Unlock("play_10");
            if (coins >= 100) Unlock("earn_100_coins");
        }
    }
}
