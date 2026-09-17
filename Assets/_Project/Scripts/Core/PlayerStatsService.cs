using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    public static class PlayerStatsService
    {
        const string GamesKey = "mig.stats.games";
        const string ScoreKey = "mig.stats.score";
        const string WinsKey = "mig.stats.wins";

        public static int GamesPlayed => Mathf.Max(0, PlayerPrefs.GetInt(GamesKey, 0));
        public static int TotalScore => Mathf.Max(0, PlayerPrefs.GetInt(ScoreKey, 0));
        public static int Wins => Mathf.Max(0, PlayerPrefs.GetInt(WinsKey, 0));
        public static int CurrentStreak { get; private set; }
        public static event Action Changed;

        public static void RecordGame(int score, bool won)
        {
            int safeScore = Mathf.Max(0, score);
            PlayerPrefs.SetInt(GamesKey, SafeAdd(GamesPlayed, 1));
            PlayerPrefs.SetInt(ScoreKey, SafeAdd(TotalScore, safeScore));
            if (won) PlayerPrefs.SetInt(WinsKey, SafeAdd(Wins, 1));
            PlayerPrefs.Save();
            Changed?.Invoke();
        }

        static int SafeAdd(int current, int amount)
        {
            if (amount <= 0) return current;
            return current > int.MaxValue - amount ? int.MaxValue : current + amount;
        }
    }
}
