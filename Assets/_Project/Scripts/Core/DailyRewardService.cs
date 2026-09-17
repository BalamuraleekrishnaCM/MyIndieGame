using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class DailyRewardService : MonoBehaviour
    {
        public static DailyRewardService Instance { get; private set; }
        public int CurrentStreak { get; private set; }
        public bool CanClaimToday => GetTodayKey() != PlayerPrefs.GetString(LastClaimKey, string.Empty);
        public int TodayReward => CalculateReward(CurrentStreak + 1);

        const string LastClaimKey = "mig.daily.last_claim";
        const string StreakKey = "mig.daily.streak";

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CurrentStreak = Mathf.Clamp(PlayerPrefs.GetInt(StreakKey, 0), 0, 7);
        }

        public bool ClaimToday()
        {
            if (!CanClaimToday || GameSession.Instance == null) return false;

            string today = GetTodayKey();
            string last = PlayerPrefs.GetString(LastClaimKey, string.Empty);
            DateTime todayDate = DateTime.ParseExact(today, "yyyy-MM-dd", null);

            if (DateTime.TryParseExact(last, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime lastDate))
                CurrentStreak = (todayDate - lastDate).TotalDays == 1 ? Mathf.Min(CurrentStreak + 1, 7) : 1;
            else
                CurrentStreak = 1;

            int reward = CalculateReward(CurrentStreak);
            GameSession.Instance.AddCoins(reward);
            PlayerPrefs.SetString(LastClaimKey, today);
            PlayerPrefs.SetInt(StreakKey, CurrentStreak);
            PlayerPrefs.Save();
            return true;
        }

        static int CalculateReward(int streak)
        {
            return 10 + Mathf.Clamp(streak - 1, 0, 6) * 5;
        }

        static string GetTodayKey() => DateTime.UtcNow.ToString("yyyy-MM-dd");
    }
}
