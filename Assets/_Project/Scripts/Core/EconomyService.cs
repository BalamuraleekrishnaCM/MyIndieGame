using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    public enum EconomySyncState
    {
        Unavailable,
        Idle,
        Loading,
        Ready,
        Queued,
        Syncing,
        Error
    }

    public interface IEconomyProvider
    {
        bool IsAvailable { get; }
        void GetBalance(Action<bool, int, string> completed);
        void ApplyReward(string transactionId, string reason, int amount, Action<bool, int, string> completed);
        void Spend(string transactionId, string reason, int amount, Action<bool, int, string> completed);
    }

    public static class EconomyService
    {
        static IEconomyProvider provider;
        static EconomySyncState state = EconomySyncState.Unavailable;

        public static IEconomyProvider Provider => provider;
        public static EconomySyncState State => state;
        public static bool IsAvailable => provider != null && provider.IsAvailable;

        public static void Configure(IEconomyProvider economyProvider)
        {
            provider = economyProvider;
            state = IsAvailable ? EconomySyncState.Idle : EconomySyncState.Unavailable;
        }

        public static void GetBalance(Action<bool, int, string> completed)
        {
            if (!IsAvailable)
            {
                state = EconomySyncState.Unavailable;
                completed?.Invoke(false, 0, "Economy provider is not configured.");
                return;
            }

            state = EconomySyncState.Loading;
            provider.GetBalance((success, balance, error) =>
            {
                state = success ? EconomySyncState.Ready : EconomySyncState.Error;
                completed?.Invoke(success, Mathf.Max(0, balance), error ?? string.Empty);
            });
        }

        public static void ApplyReward(string transactionId, string reason, int amount, Action<bool, int, string> completed)
        {
            if (string.IsNullOrWhiteSpace(transactionId) || string.IsNullOrWhiteSpace(reason) || amount <= 0)
            {
                completed?.Invoke(false, 0, "Invalid economy transaction.");
                return;
            }

            if (!IsAvailable)
            {
                state = EconomySyncState.Queued;
                completed?.Invoke(false, 0, "Economy provider is not configured; reward must be reconciled later.");
                return;
            }

            state = EconomySyncState.Syncing;
            provider.ApplyReward(transactionId, reason, amount, (success, balance, error) =>
            {
                state = success ? EconomySyncState.Ready : EconomySyncState.Error;
                completed?.Invoke(success, Mathf.Max(0, balance), error ?? string.Empty);
            });
        }

        public static void Spend(string transactionId, string reason, int amount, Action<bool, int, string> completed)
        {
            if (string.IsNullOrWhiteSpace(transactionId) || string.IsNullOrWhiteSpace(reason) || amount <= 0)
            {
                completed?.Invoke(false, 0, "Invalid economy transaction.");
                return;
            }

            if (!IsAvailable)
            {
                completed?.Invoke(false, 0, "Economy provider is not configured.");
                return;
            }

            state = EconomySyncState.Syncing;
            provider.Spend(transactionId, reason, amount, (success, balance, error) =>
            {
                state = success ? EconomySyncState.Ready : EconomySyncState.Error;
                completed?.Invoke(success, Mathf.Max(0, balance), error ?? string.Empty);
            });
        }
    }
}
