using System;
using System.Collections.Generic;

namespace MyIndieGame.Services
{
    public enum LeaderboardSubmissionStatus
    {
        Accepted,
        Rejected,
        Offline,
        Retryable,
        Unavailable
    }

    [Serializable]
    public sealed class LeaderboardEntry
    {
        public string GameId;
        public string PlayerId;
        public long Score;
        public int Rank;
        public long ServerTimestampUnix;
        public bool ServerConfirmed;
    }

    [Serializable]
    public sealed class LeaderboardSubmission
    {
        public string SubmissionId;
        public string GameId;
        public string PlayerId;
        public long Score;
    }

    public readonly struct LeaderboardSubmissionResult
    {
        public readonly LeaderboardSubmissionStatus Status;
        public readonly string SubmissionId;
        public readonly LeaderboardEntry Entry;
        public readonly string Error;

        public LeaderboardSubmissionResult(
            LeaderboardSubmissionStatus status,
            string submissionId,
            LeaderboardEntry entry = null,
            string error = null)
        {
            Status = status;
            SubmissionId = submissionId;
            Entry = entry;
            Error = error;
        }
    }

    public interface ILeaderboardProvider
    {
        LeaderboardSubmissionResult Submit(LeaderboardSubmission submission);
        IReadOnlyList<LeaderboardEntry> GetTop(string gameId, int limit);
    }

    public interface ILeaderboardService
    {
        LeaderboardSubmissionResult SubmitBestScore(string gameId, string playerId, long score);
        IReadOnlyList<LeaderboardEntry> GetTop(string gameId, int limit = 20);
    }

    /// <summary>
    /// Provider-neutral leaderboard boundary. The client may cache local best scores,
    /// but authoritative rank must come from the configured provider.
    /// </summary>
    public sealed class LeaderboardService : ILeaderboardService
    {
        private readonly ILeaderboardProvider provider;
        private readonly Dictionary<string, long> localBestScores = new();

        public LeaderboardService(ILeaderboardProvider provider)
        {
            this.provider = provider;
        }

        public LeaderboardSubmissionResult SubmitBestScore(string gameId, string playerId, long score)
        {
            if (string.IsNullOrWhiteSpace(gameId) || string.IsNullOrWhiteSpace(playerId) || score < 0)
                return new LeaderboardSubmissionResult(LeaderboardSubmissionStatus.Rejected, null, error: "Invalid leaderboard submission.");

            var key = gameId + ":" + playerId;
            if (localBestScores.TryGetValue(key, out var best) && score <= best)
                return new LeaderboardSubmissionResult(LeaderboardSubmissionStatus.Rejected, CreateSubmissionId(gameId, playerId, score), error: "Score does not exceed local best.");

            localBestScores[key] = score;
            var submissionId = CreateSubmissionId(gameId, playerId, score);
            var submission = new LeaderboardSubmission
            {
                SubmissionId = submissionId,
                GameId = gameId,
                PlayerId = playerId,
                Score = score
            };

            if (provider == null)
                return new LeaderboardSubmissionResult(LeaderboardSubmissionStatus.Offline, submissionId, error: "Leaderboard provider unavailable.");

            return provider.Submit(submission);
        }

        public IReadOnlyList<LeaderboardEntry> GetTop(string gameId, int limit = 20)
        {
            if (provider == null || string.IsNullOrWhiteSpace(gameId) || limit <= 0)
                return Array.Empty<LeaderboardEntry>();

            return provider.GetTop(gameId, Math.Min(limit, 100));
        }

        private static string CreateSubmissionId(string gameId, string playerId, long score)
        {
            return gameId + ":" + playerId + ":" + score;
        }
    }
}
