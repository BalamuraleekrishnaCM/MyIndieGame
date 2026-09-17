using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyIndieGame.Core
{
    [Serializable]
    public sealed class LeaderboardEntry
    {
        public string PlayerId;
        public string DisplayName;
        public int Score;
    }

    public static class LeaderboardService
    {
        const string LocalScorePrefix = "mig.leaderboard.";

        public static int GetLocalBest(string gameId)
        {
            return PlayerPrefs.GetInt(LocalScorePrefix + gameId, 0);
        }

        public static bool SubmitLocal(string gameId, int score)
        {
            if (string.IsNullOrWhiteSpace(gameId) || score <= GetLocalBest(gameId)) return false;
            PlayerPrefs.SetInt(LocalScorePrefix + gameId, score);
            PlayerPrefs.Save();
            return true;
        }

        public static IReadOnlyList<LeaderboardEntry> GetCached(string gameId)
        {
            // Remote leaderboard providers can populate this cache later.
            return Array.Empty<LeaderboardEntry>();
        }
    }
}
