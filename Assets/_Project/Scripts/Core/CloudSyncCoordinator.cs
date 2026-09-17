using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    public static class CloudSyncCoordinator
    {
        const string PendingKey = "mig.cloud.pending";

        public static bool HasPendingSync => PlayerPrefs.GetInt(PendingKey, 0) == 1;

        public static CloudPlayerData CreateLocalSnapshot()
        {
            var identity = PlayerIdentityService.Current;
            if (identity == null || !identity.IsAuthenticated) return null;

            return new CloudPlayerData
            {
                UserId = identity.UserId,
                DisplayName = identity.DisplayName,
                Coins = GameSession.Instance != null ? GameSession.Instance.Coins : 0,
                GamesPlayed = GameSession.Instance != null ? GameSession.Instance.TotalGamesPlayed : 0,
                TotalScore = PlayerStatsService.TotalScore,
                Wins = PlayerStatsService.Wins,
                UpdatedAtUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }

        public static void PushLocalSnapshot(Action<bool, string> completed)
        {
            CloudPlayerData snapshot = CreateLocalSnapshot();
            if (snapshot == null)
            {
                MarkPending();
                completed?.Invoke(false, "Player is not authenticated.");
                return;
            }

            CloudSyncService.Push(success =>
            {
                if (success) ClearPending();
                else MarkPending();
                completed?.Invoke(success, success ? string.Empty : "Cloud push failed.");
            });
        }

        public static void MarkPending()
        {
            PlayerPrefs.SetInt(PendingKey, 1);
            PlayerPrefs.Save();
        }

        public static void ClearPending()
        {
            PlayerPrefs.SetInt(PendingKey, 0);
            PlayerPrefs.Save();
        }
    }
}
