using UnityEngine;

namespace MyIndieGame.Core
{
    public static class BestScoreStore
    {
        public static int Get(string gameId) => PlayerPrefs.GetInt("mig.best." + gameId, 0);

        public static bool Submit(string gameId, int score)
        {
            if (score <= Get(gameId)) return false;
            PlayerPrefs.SetInt("mig.best." + gameId, score);
            PlayerPrefs.Save();
            return true;
        }
    }
}
