using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    public static class PlayerStatsService
    {
        const string GamesKey = "mig.stats.games";
        const string ScoreKey = "mig.stats.score";
        const string WinsKey = "mig.stats.wins";

        public static int GamesPlayed => PlayerPrefs.GetInt(GamesKey, 0);
        public static int TotalScore => PlayerPrefs.GetInt(ScoreKey, 0);
        public static int Wins => PlayerPrefs.GetInt(WinsKey, 0);
        public static int CurrentStreak { get; private set; }
        public static event Action Changed;

        public static void RecordGame(int score, bool won)
        {
            PlayerPrefs.SetInt(GamesKey, GamesPlayed + 1);
            PlayerPrefs.SetInt(ScoreKey, TotalScore + Mathf.Max(0, score));
            if (won) PlayerPrefs.SetInt(WinsKey, Wins + 1);
            PlayerPrefs.Save();
            Changed?.Invoke();
        }
    }
}
