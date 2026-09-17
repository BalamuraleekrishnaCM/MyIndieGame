using System.Collections.Generic;

namespace MyIndieGame.Services
{
    /// <summary>
    /// Development-safe provider placeholder. Replace with a real authenticated
    /// backend adapter before enabling production leaderboard submissions.
    /// </summary>
    public sealed class LocalLeaderboardProvider : ILeaderboardProvider
    {
        private readonly Dictionary<string, List<LeaderboardEntry>> entries = new();

        public LeaderboardSubmissionResult Submit(LeaderboardSubmission submission)
        {
            if (!entries.TryGetValue(submission.GameId, out var gameEntries))
            {
                gameEntries = new List<LeaderboardEntry>();
                entries[submission.GameId] = gameEntries;
            }

            for (var i = 0; i < gameEntries.Count; i++)
            {
                if (gameEntries[i].PlayerId == submission.PlayerId && gameEntries[i].Score >= submission.Score)
                    return new LeaderboardSubmissionResult(LeaderboardSubmissionStatus.Rejected, submission.SubmissionId, error: "Existing score is higher or equal.");
            }

            gameEntries.RemoveAll(e => e.PlayerId == submission.PlayerId);
            gameEntries.Add(new LeaderboardEntry
            {
                GameId = submission.GameId,
                PlayerId = submission.PlayerId,
                Score = submission.Score,
                ServerConfirmed = false,
                ServerTimestampUnix = 0
            });
            gameEntries.Sort((a, b) => b.Score.CompareTo(a.Score));

            for (var i = 0; i < gameEntries.Count; i++)
                gameEntries[i].Rank = i + 1;

            var entry = gameEntries.Find(e => e.PlayerId == submission.PlayerId && e.Score == submission.Score);
            return new LeaderboardSubmissionResult(LeaderboardSubmissionStatus.Accepted, submission.SubmissionId, entry);
        }

        public IReadOnlyList<LeaderboardEntry> GetTop(string gameId, int limit)
        {
            if (!entries.TryGetValue(gameId, out var gameEntries))
                return new List<LeaderboardEntry>();

            var count = System.Math.Min(limit, gameEntries.Count);
            return gameEntries.GetRange(0, count);
        }
    }
}
