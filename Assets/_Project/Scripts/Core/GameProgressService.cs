using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyIndieGame.Core
{
    /// <summary>
    /// Small provider-neutral persistence layer for the mini-game hub.
    /// Stores only non-authoritative local progress: best scores, play counts,
    /// and whether a game has been discovered. Online economy/leaderboards
    /// remain owned by their respective services.
    /// </summary>
    public static class GameProgressService
    {
        const string BestPrefix = "mig.progress.best.";
        const string PlaysPrefix = "mig.progress.plays.";
        const string DiscoveredPrefix = "mig.progress.discovered.";

        public static event Action<string> ProgressChanged;

        public static int GetBest(string gameId)
        {
            return PlayerPrefs.GetInt(Key(BestPrefix, gameId), 0);
        }

        public static int GetPlayCount(string gameId)
        {
            return PlayerPrefs.GetInt(Key(PlaysPrefix, gameId), 0);
        }

        public static bool IsDiscovered(string gameId)
        {
            return PlayerPrefs.GetInt(Key(DiscoveredPrefix, gameId), 0) == 1;
        }

        public static bool RecordGameFinished(string gameId, int score)
        {
            if (!Valid(gameId) || score < 0) return false;

            string id = gameId.Trim();
            int oldBest = GetBest(id);
            int plays = GetPlayCount(id) + 1;

            PlayerPrefs.SetInt(Key(PlaysPrefix, id), plays);
            PlayerPrefs.SetInt(Key(DiscoveredPrefix, id), 1);

            bool newBest = score > oldBest;
            if (newBest) PlayerPrefs.SetInt(Key(BestPrefix, id), score);

            PlayerPrefs.Save();
            ProgressChanged?.Invoke(id);
            return newBest;
        }

        public static void MarkDiscovered(string gameId)
        {
            if (!Valid(gameId)) return;
            string id = gameId.Trim();
            if (IsDiscovered(id)) return;
            PlayerPrefs.SetInt(Key(DiscoveredPrefix, id), 1);
            PlayerPrefs.Save();
            ProgressChanged?.Invoke(id);
        }

        public static void ResetGame(string gameId)
        {
            if (!Valid(gameId)) return;
            string id = gameId.Trim();
            PlayerPrefs.DeleteKey(Key(BestPrefix, id));
            PlayerPrefs.DeleteKey(Key(PlaysPrefix, id));
            PlayerPrefs.DeleteKey(Key(DiscoveredPrefix, id));
            PlayerPrefs.Save();
            ProgressChanged?.Invoke(id);
        }

        public static void ResetAll(IEnumerable<string> gameIds)
        {
            if (gameIds == null) return;
            foreach (string gameId in gameIds) ResetGame(gameId);
        }

        static bool Valid(string gameId) => !string.IsNullOrWhiteSpace(gameId);
        static string Key(string prefix, string gameId) => prefix + (gameId ?? string.Empty).Trim();
    }
}
