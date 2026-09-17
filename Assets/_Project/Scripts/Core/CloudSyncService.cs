using System;

namespace MyIndieGame.Core
{
    public enum CloudSyncState
    {
        Idle,
        Pulling,
        Pushing,
        Synced,
        Offline,
        Error
    }

    public interface ICloudSyncProvider
    {
        bool IsAuthenticated { get; }
        void Pull(Action<CloudPlayerData, string> completed);
        void Push(CloudPlayerData data, Action<bool, string> completed);
    }

    public static class CloudSyncService
    {
        public static ICloudSyncProvider Provider { get; private set; }
        public static CloudSyncState State { get; private set; } = CloudSyncState.Offline;
        public static CloudPlayerData LastCloudData { get; private set; }
        public static string LastError { get; private set; }

        public static event Action<CloudSyncState> StateChanged;

        public static bool IsAvailable => Provider != null && Provider.IsAuthenticated;

        public static void Configure(ICloudSyncProvider provider)
        {
            Provider = provider;
            LastCloudData = null;
            LastError = string.Empty;
            SetState(IsAvailable ? CloudSyncState.Idle : CloudSyncState.Offline);
        }

        public static void Pull(Action<bool, CloudPlayerData, string> completed)
        {
            if (!IsAvailable)
            {
                SetState(CloudSyncState.Offline);
                completed?.Invoke(false, null, "Cloud sync is unavailable.");
                return;
            }

            SetState(CloudSyncState.Pulling);
            Provider.Pull((data, error) =>
            {
                if (data != null)
                {
                    LastCloudData = data;
                    LastError = string.Empty;
                    SetState(CloudSyncState.Synced);
                    completed?.Invoke(true, data, string.Empty);
                    return;
                }

                LastError = string.IsNullOrWhiteSpace(error) ? "Cloud pull failed." : error;
                SetState(CloudSyncState.Error);
                completed?.Invoke(false, null, LastError);
            });
        }

        public static void Push(CloudPlayerData data, Action<bool, string> completed)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.UserId))
            {
                completed?.Invoke(false, "Cloud player data is invalid.");
                return;
            }

            if (!IsAvailable)
            {
                SetState(CloudSyncState.Offline);
                completed?.Invoke(false, "Cloud sync is unavailable.");
                return;
            }

            SetState(CloudSyncState.Pushing);
            Provider.Push(data, (success, error) =>
            {
                if (success)
                {
                    LastCloudData = data;
                    LastError = string.Empty;
                    SetState(CloudSyncState.Synced);
                    completed?.Invoke(true, string.Empty);
                    return;
                }

                LastError = string.IsNullOrWhiteSpace(error) ? "Cloud push failed." : error;
                SetState(CloudSyncState.Error);
                completed?.Invoke(false, LastError);
            });
        }

        static void SetState(CloudSyncState state)
        {
            State = state;
            StateChanged?.Invoke(state);
        }
    }
}
