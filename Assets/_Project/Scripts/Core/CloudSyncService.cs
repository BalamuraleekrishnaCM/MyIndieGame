using System;

namespace MyIndieGame.Core
{
    public interface ICloudSyncProvider
    {
        bool IsAuthenticated { get; }
        void SignIn(Action<bool, string> completed);
        void SignOut(Action<bool> completed);
        void Pull(Action<bool, string> completed);
        void Push(Action<bool, string> completed);
    }

    public static class CloudSyncService
    {
        public static ICloudSyncProvider Provider { get; private set; }
        public static bool IsAvailable => Provider != null;

        public static void Configure(ICloudSyncProvider provider) => Provider = provider;
        public static void Pull(Action<bool, string> completed) => Provider?.Pull(completed);
        public static void Push(Action<bool, string> completed) => Provider?.Push(completed);
    }
}
