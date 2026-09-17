using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    public static class MissionService
    {
        const string DayKey = "mig.mission.day";
        const string GamesKey = "mig.mission.games";
        const string ScoreKey = "mig.mission.score";
        const string ClaimedKey = "mig.mission.claimed";

        public static int GamesToday { get { ResetIfNewDay(); return Mathf.Max(0, PlayerPrefs.GetInt(GamesKey, 0)); } }
        public static int ScoreToday { get { ResetIfNewDay(); return Mathf.Max(0, PlayerPrefs.GetInt(ScoreKey, 0)); } }
        public static bool RewardClaimed { get { ResetIfNewDay(); return PlayerPrefs.GetInt(ClaimedKey, 0) == 1; } }
        public static bool Completed => GamesToday >= 3 && ScoreToday >= 100;

        public static void RecordGame(int score)
        {
            ResetIfNewDay();
            int games = Mathf.Max(0, PlayerPrefs.GetInt(GamesKey, 0));
            int totalScore = Mathf.Max(0, PlayerPrefs.GetInt(ScoreKey, 0));
            PlayerPrefs.SetInt(GamesKey, SafeAdd(games, 1));
            PlayerPrefs.SetInt(ScoreKey, SafeAdd(totalScore, Mathf.Max(0, score)));
            PlayerPrefs.Save();
        }

        public static int ClaimReward()
        {
            ResetIfNewDay();
            if (!Completed || RewardClaimed || GameSession.Instance == null) return 0;

            const int reward = 20;
            PlayerPrefs.SetInt(ClaimedKey, 1);
            PlayerPrefs.Save();
            GameSession.Instance.AddCoins(reward);
            AnalyticsService.RecordRewardClaimed("daily_mission");
            return reward;
        }

        static void ResetIfNewDay()
        {
            string today = DateTime.UtcNow.ToString("yyyyMMdd");
            if (PlayerPrefs.GetString(DayKey, string.Empty) == today) return;
            PlayerPrefs.SetString(DayKey, today);
            PlayerPrefs.SetInt(GamesKey, 0);
            PlayerPrefs.SetInt(ScoreKey, 0);
            PlayerPrefs.SetInt(ClaimedKey, 0);
            PlayerPrefs.Save();
        }

        static int SafeAdd(int current, int amount)
        {
            if (amount <= 0) return current;
            return current > int.MaxValue - amount ? int.MaxValue : current + amount;
        }
    }
}
