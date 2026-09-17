using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyIndieGame.Core
{
    public static class LeaderboardService
    {
        const string LocalScorePrefix = "mig.leaderboard.";
        const string IdempotencyPrefix = "mig.leaderboard.submission.";
        static readonly Dictionary<string, LeaderboardPage> Cache = new Dictionary<string, LeaderboardPage>();

        public static ILeaderboardProvider Provider { get; private set; }
        public static LeaderboardState State { get; private set; } = LeaderboardState.Offline;
        public static string LastError { get; private set; } = string.Empty;
        public static LeaderboardPage LastPage { get; private set; }
        public static event Action<LeaderboardState> StateChanged;
        public static bool IsAvailable => Provider != null && Provider.IsAvailable;

        public static void Configure(ILeaderboardProvider provider)
        {
            Provider = provider;
            Cache.Clear();
            LastPage = null;
            LastError = string.Empty;
            SetState(IsAvailable ? LeaderboardState.Ready : LeaderboardState.Offline);
        }

        public static int GetLocalBest(string gameId)
        {
            return string.IsNullOrWhiteSpace(gameId) ? 0 : PlayerPrefs.GetInt(LocalScorePrefix + gameId.Trim(), 0);
        }

        public static bool SubmitLocal(string gameId, int score)
        {
            if (string.IsNullOrWhiteSpace(gameId) || score <= GetLocalBest(gameId)) return false;
            PlayerPrefs.SetInt(LocalScorePrefix + gameId.Trim(), score);
            PlayerPrefs.Save();
            return true;
        }

        public static void GetTop(string gameId, string seasonId, int limit, Action<bool, LeaderboardPage, string> completed)
        {
            if (!ValidGame(gameId)) { Fail(completed, "Game ID is required."); return; }
            if (!IsAvailable)
            {
                var cached = GetCached(gameId, seasonId);
                completed?.Invoke(cached != null, cached, cached == null ? "Leaderboard is unavailable." : string.Empty);
                return;
            }
            SetState(LeaderboardState.Loading);
            Provider.GetTop(gameId.Trim(), seasonId ?? string.Empty, Mathf.Clamp(limit, 1, 100), (page, error) =>
            {
                if (page != null) { Store(page); LastPage = page; LastError = string.Empty; SetState(LeaderboardState.Ready); completed?.Invoke(true, page, string.Empty); }
                else Fail(completed, Error(error, "Leaderboard request failed."));
            });
        }

        public static void GetAroundPlayer(string gameId, string seasonId, int limit, Action<bool, LeaderboardPage, string> completed)
        {
            if (!ValidGame(gameId)) { Fail(completed, "Game ID is required."); return; }
            if (!IsAvailable)
            {
                var cached = GetCached(gameId, seasonId);
                completed?.Invoke(cached != null, cached, cached == null ? "Leaderboard is unavailable." : string.Empty);
                return;
            }
            SetState(LeaderboardState.Loading);
            Provider.GetAroundPlayer(gameId.Trim(), seasonId ?? string.Empty, Mathf.Clamp(limit, 1, 100), (page, error) =>
            {
                if (page != null) { Store(page); LastPage = page; LastError = string.Empty; SetState(LeaderboardState.Ready); completed?.Invoke(true, page, string.Empty); }
                else Fail(completed, Error(error, "Leaderboard request failed."));
            });
        }

        public static void SubmitScore(string gameId, string seasonId, int score, Action<bool, string> completed)
        {
            if (!ValidGame(gameId) || score < 0) { completed?.Invoke(false, "Invalid leaderboard submission."); return; }
            if (score <= GetLocalBest(gameId)) { completed?.Invoke(false, "Score is not a new personal best."); return; }
            SubmitLocal(gameId, score);
            if (!IsAvailable) { completed?.Invoke(false, "Score saved locally; remote leaderboard is unavailable."); return; }

            string key = IdempotencyPrefix + gameId.Trim() + "." + (seasonId ?? string.Empty) + "." + score;
            if (PlayerPrefs.GetInt(key, 0) == 1) { completed?.Invoke(true, string.Empty); return; }
            SetState(LeaderboardState.Submitting);
            Provider.SubmitScore(gameId.Trim(), seasonId ?? string.Empty, score, key, (success, error) =>
            {
                if (success)
                {
                    PlayerPrefs.SetInt(key, 1);
                    PlayerPrefs.Save();
                    LastError = string.Empty;
                    SetState(LeaderboardState.Ready);
                    completed?.Invoke(true, string.Empty);
                }
                else FailSubmit(completed, Error(error, "Score submission failed."));
            });
        }

        public static IReadOnlyList<LeaderboardEntry> GetCachedEntries(string gameId, string seasonId = "")
        {
            var page = GetCached(gameId, seasonId);
            return page == null ? Array.Empty<LeaderboardEntry>() : page.Entries;
        }

        static LeaderboardPage GetCached(string gameId, string seasonId)
        {
            if (string.IsNullOrWhiteSpace(gameId)) return null;
            Cache.TryGetValue(Key(gameId, seasonId), out var page);
            return page;
        }

        static void Store(LeaderboardPage page)
        {
            if (page == null || string.IsNullOrWhiteSpace(page.GameId)) return;
            page.Entries = page.Entries ?? new List<LeaderboardEntry>();
            Cache[Key(page.GameId, page.SeasonId)] = page;
        }

        static string Key(string gameId, string seasonId) => gameId.Trim() + "|" + (seasonId ?? string.Empty);
        static bool ValidGame(string gameId) => !string.IsNullOrWhiteSpace(gameId);
        static string Error(string error, string fallback) => string.IsNullOrWhiteSpace(error) ? fallback : error;

        static void Fail(Action<bool, LeaderboardPage, string> completed, string error)
        {
            LastError = error;
            SetState(LeaderboardState.Error);
            completed?.Invoke(false, null, error);
        }

        static void FailSubmit(Action<bool, string> completed, string error)
        {
            LastError = error;
            SetState(LeaderboardState.Error);
            completed?.Invoke(false, error);
        }

        static void SetState(LeaderboardState state)
        {
            State = state;
            StateChanged?.Invoke(state);
        }
    }
}
