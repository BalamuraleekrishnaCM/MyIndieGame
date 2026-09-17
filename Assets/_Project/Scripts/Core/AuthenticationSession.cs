using System;

namespace MyIndieGame.Core
{
    [Serializable]
    public sealed class AuthenticationSession
    {
        public string UserId;
        public string DisplayName;
        public DateTime ExpiresAtUtc;

        public bool HasExpiry => ExpiresAtUtc != DateTime.MinValue;
        public bool IsExpired => HasExpiry && DateTime.UtcNow >= ExpiresAtUtc;
    }
}
