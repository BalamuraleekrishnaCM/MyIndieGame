using System;
using System.Collections.Generic;

namespace MyIndieGame.Core
{
    [Serializable]
    public sealed class LeaderboardEntry
    {
        public string PlayerId;
        public string DisplayName;
        public int Score;
        public int Rank;
    }

    [Serializable]
    public sealed class LeaderboardPage
    {
        public string GameId;
        public string SeasonId;
        public int Limit;
        public int Offset;
        public int TotalCount;
        public List<LeaderboardEntry> Entries = new List<LeaderboardEntry>();
    }

    public enum LeaderboardState
    {
        Offline,
        Loading,
        Ready,
        Submitting,
        Error
    }

    public interface ILeaderboardProvider
    {
        bool IsAvailable { get; }
        void GetTop(string gameId, string seasonId, int limit, Action<LeaderboardPage, string> completed);
        void GetAroundPlayer(string gameId, string seasonId, int limit, Action<LeaderboardPage, string> completed);
        void SubmitScore(string gameId, string seasonId, int score, string idempotencyKey, Action<bool, string> completed);
    }
}
