using System;

namespace MyIndieGame.Core
{
    public interface IAuthenticationProvider
    {
        bool IsAvailable { get; }
        void SignIn(Action<PlayerIdentity, string> completed);
        void SignOut(Action<bool> completed);
    }

    public static class AuthenticationService
    {
        public static IAuthenticationProvider Provider { get; private set; }
        public static bool IsSignedIn => PlayerIdentityService.Current != null && PlayerIdentityService.Current.IsAuthenticated;

        public static void Configure(IAuthenticationProvider provider)
        {
            Provider = provider;
            PlayerIdentityService.Initialize();
        }

        public static void SignIn(Action<bool, string> completed)
        {
            if (Provider == null || !Provider.IsAvailable)
            {
                completed?.Invoke(false, "Authentication provider is not configured.");
                return;
            }
            Provider.SignIn((identity, error) =>
            {
                if (identity == null || string.IsNullOrWhiteSpace(identity.UserId))
                {
                    completed?.Invoke(false, string.IsNullOrWhiteSpace(error) ? "Sign-in failed." : error);
                    return;
                }
                PlayerIdentityService.SetAuthenticated(identity.UserId, identity.DisplayName);
                completed?.Invoke(true, string.Empty);
            });
        }

        public static void SignOut(Action<bool> completed)
        {
            if (Provider == null || !Provider.IsAvailable)
            {
                PlayerIdentityService.SignOut();
                completed?.Invoke(true);
                return;
            }
            Provider.SignOut(success =>
            {
                if (success) PlayerIdentityService.SignOut();
                completed?.Invoke(success);
            });
        }
    }
}
