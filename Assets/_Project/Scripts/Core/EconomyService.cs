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
        static bool operationInFlight;

        public static IEconomyProvider Provider => provider;
        public static EconomySyncState State => state;
        public static bool IsAvailable => provider != null && provider.IsAvailable;
        public static bool IsBusy => operationInFlight;

        public static void Configure(IEconomyProvider economyProvider)
        {
            provider = economyProvider;
            operationInFlight = false;
            SetState(IsAvailable ? EconomySyncState.Idle : EconomySyncState.Unavailable);
        }

        public static void GetBalance(Action<bool, int, string> completed)
        {
            if (!TryStart(EconomySyncState.Loading, completed)) return;
            provider.GetBalance((success, balance, error) =>
            {
                Complete(success ? EconomySyncState.Ready : EconomySyncState.Error, completed, success, Mathf.Max(0, balance), error);
            });
        }

        public static void ApplyReward(string transactionId, string reason, int amount, Action<bool, int, string> completed)
        {
            if (!ValidateTransaction(transactionId, reason, amount, completed)) return;
            if (!IsAvailable)
            {
                SetState(EconomySyncState.Queued);
                completed?.Invoke(false, 0, "Economy provider is unavailable; persist the transaction intent for reconciliation.");
                return;
            }
            if (!EconomyTransactionGuard.TryBegin(transactionId))
            {
                completed?.Invoke(false, 0, "Transaction is already in progress or was already submitted.");
                return;
            }
            if (!TryStart(EconomySyncState.Syncing, completed, transactionId)) return;

            provider.ApplyReward(transactionId.Trim(), reason.Trim(), amount, (success, balance, error) =>
            {
                if (!success) EconomyTransactionGuard.Forget(transactionId);
                Complete(success ? EconomySyncState.Ready : EconomySyncState.Error, completed, success, Mathf.Max(0, balance), error);
            });
        }

        public static void Spend(string transactionId, string reason, int amount, Action<bool, int, string> completed)
        {
            if (!ValidateTransaction(transactionId, reason, amount, completed)) return;
            if (!IsAvailable)
            {
                SetState(EconomySyncState.Unavailable);
                completed?.Invoke(false, 0, "Economy provider is unavailable.");
                return;
            }
            if (!EconomyTransactionGuard.TryBegin(transactionId))
            {
                completed?.Invoke(false, 0, "Transaction is already in progress or was already submitted.");
                return;
            }
            if (!TryStart(EconomySyncState.Syncing, completed, transactionId)) return;

            provider.Spend(transactionId.Trim(), reason.Trim(), amount, (success, balance, error) =>
            {
                if (!success) EconomyTransactionGuard.Forget(transactionId);
                Complete(success ? EconomySyncState.Ready : EconomySyncState.Error, completed, success, Mathf.Max(0, balance), error);
            });
        }

        static bool ValidateTransaction(string transactionId, string reason, int amount, Action<bool, int, string> completed)
        {
            if (string.IsNullOrWhiteSpace(transactionId) || string.IsNullOrWhiteSpace(reason) || amount <= 0 || amount == int.MaxValue)
            {
                completed?.Invoke(false, 0, "Invalid economy transaction.");
                return false;
            }
            return true;
        }

        static bool TryStart(EconomySyncState nextState, Action<bool, int, string> completed, string transactionId = null)
        {
            if (!IsAvailable)
            {
                SetState(EconomySyncState.Unavailable);
                completed?.Invoke(false, 0, "Economy provider is not configured or authenticated.");
                if (!string.IsNullOrWhiteSpace(transactionId)) EconomyTransactionGuard.Forget(transactionId);
                return false;
            }
            if (operationInFlight)
            {
                completed?.Invoke(false, 0, "Another economy operation is already in progress.");
                if (!string.IsNullOrWhiteSpace(transactionId)) EconomyTransactionGuard.Forget(transactionId);
                return false;
            }
            operationInFlight = true;
            SetState(nextState);
            return true;
        }

        static void Complete(EconomySyncState nextState, Action<bool, int, string> completed, bool success, int balance, string error)
        {
            operationInFlight = false;
            SetState(nextState);
            completed?.Invoke(success, balance, string.IsNullOrWhiteSpace(error) ? string.Empty : error);
        }

        static void SetState(EconomySyncState nextState)
        {
            state = nextState;
        }
    }
}
