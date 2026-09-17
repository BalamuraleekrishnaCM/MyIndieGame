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

        public static int GamesToday => GetInt(GamesKey);
        public static int ScoreToday => GetInt(ScoreKey);
        public static bool RewardClaimed => PlayerPrefs.GetInt(ClaimedKey, 0) == 1;
        public static bool Completed => GamesToday >= 3 && ScoreToday >= 100;

        public static void RecordGame(int score)
        {
            ResetIfNewDay();
            PlayerPrefs.SetInt(GamesKey, GamesToday + 1);
            PlayerPrefs.SetInt(ScoreKey, ScoreToday + Mathf.Max(0, score));
            PlayerPrefs.Save();
        }

        public static int ClaimReward()
        {
            ResetIfNewDay();
            if (!Completed || RewardClaimed) return 0;
            const int reward = 20;
            PlayerPrefs.SetInt(ClaimedKey, 1);
            PlayerPrefs.Save();
            if (GameSession.Instance != null) GameSession.Instance.AddCoins(reward);
            AnalyticsService.RecordRewardClaimed("daily_mission");
            return reward;
        }

        static void ResetIfNewDay()
        {
            string today = DateTime.Now.ToString("yyyyMMdd");
            if (PlayerPrefs.GetString(DayKey, "") == today) return;
            PlayerPrefs.SetString(DayKey, today);
            PlayerPrefs.SetInt(GamesKey, 0);
            PlayerPrefs.SetInt(ScoreKey, 0);
            PlayerPrefs.SetInt(ClaimedKey, 0);
            PlayerPrefs.Save();
        }

        static int GetInt(string key) { ResetIfNewDay(); return PlayerPrefs.GetInt(key, 0); }
    }
}
