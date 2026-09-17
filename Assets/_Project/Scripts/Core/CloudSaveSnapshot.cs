using UnityEngine;

namespace MyIndieGame.Core
{
    public static class CloudSaveSnapshot
    {
        public static CloudPlayerData Capture()
        {
            PlayerIdentity identity = PlayerIdentityService.Current;
            return new CloudPlayerData
            {
                UserId = identity?.UserId ?? string.Empty,
                DisplayName = identity?.DisplayName ?? "Player",
                Coins = GameSession.Instance?.Coins ?? 0,
                GamesPlayed = GameSession.Instance?.TotalGamesPlayed ?? 0,
                TotalScore = PlayerStatsService.TotalScore,
                Wins = PlayerStatsService.Wins,
                Version = CloudSyncService.LastCloudData?.Version ?? 0,
                UpdatedAtUnix = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }
    }
}
