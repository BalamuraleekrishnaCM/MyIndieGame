using System;

namespace MyIndieGame.Core
{
    /// <summary>
    /// Development-only provider that demonstrates the authentication boundary without a backend.
    /// It must not be treated as production authentication.
    /// </summary>
    public sealed class AnonymousIdentityProvider : IAuthenticationProvider
    {
        public bool IsAvailable => true;

        public void SignIn(Action<PlayerIdentity, string> completed)
        {
            completed?.Invoke(new PlayerIdentity
            {
                UserId = "local-" + Guid.NewGuid().ToString("N"),
                DisplayName = "Player",
                IsAuthenticated = true
            }, string.Empty);
        }

        public void SignOut(Action<bool> completed) => completed?.Invoke(true);
    }
}
