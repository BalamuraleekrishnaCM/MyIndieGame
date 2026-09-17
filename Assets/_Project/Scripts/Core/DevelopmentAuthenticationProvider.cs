using System;

namespace MyIndieGame.Core
{
    /// <summary>
    /// Development-only provider. Never use this as production authentication.
    /// </summary>
    public sealed class DevelopmentAuthenticationProvider : IAuthenticationProvider
    {
        public bool IsAvailable => true;

        public void SignIn(Action<PlayerIdentity, string> completed)
        {
            completed?.Invoke(new PlayerIdentity
            {
                UserId = "dev-player",
                DisplayName = "Dev Player",
                IsAuthenticated = true
            }, string.Empty);
        }

        public void SignOut(Action<bool> completed)
        {
            completed?.Invoke(true);
        }
    }
}
