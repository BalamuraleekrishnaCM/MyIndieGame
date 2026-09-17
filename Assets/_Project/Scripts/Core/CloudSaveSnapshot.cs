using UnityEngine;

namespace MyIndieGame.Core
{
    public static class CloudSaveSnapshot
    {
        public static CloudPlayerData Capture()
        {
            PlayerIdentity identity = PlayerIdentityService.Current;
            long baseVersion = CloudSyncService.LastCloudData != null
                ? CloudSyncService.LastCloudData.Version
                : 0;

            return new CloudPlayerData
            {
                UserId = identity != null && identity.IsAuthenticated ? identity.UserId : string.Empty,
                DisplayName = identity != null ? identity.DisplayName : "Player",
                Coins = GameSession.Instance != null ? GameSession.Instance.Coins : 0,
                GamesPlayed = GameSession.Instance != null ? GameSession.Instance.TotalGamesPlayed : 0,
                TotalScore = PlayerStatsService.TotalScore,
                Wins = PlayerStatsService.Wins,
                // Version 0 is the initial unknown state. Providers should use the
                // previous server version for compare-and-swap and assign the
                // canonical server version after a successful write.
                Version = baseVersion,
                UpdatedAtUnix = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }
    }
}
