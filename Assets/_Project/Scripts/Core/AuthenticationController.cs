using System;

namespace MyIndieGame.Core
{
    public sealed class AuthenticationController
    {
        public AuthenticationState State { get; private set; } = AuthenticationState.SignedOut;
        public AuthenticationSession Session { get; private set; }
        public string LastError { get; private set; }

        public event Action Changed;

        public void SignIn()
        {
            if (State == AuthenticationState.SigningIn || State == AuthenticationState.SignedIn) return;

            State = AuthenticationState.SigningIn;
            LastError = string.Empty;
            Changed?.Invoke();

            AuthenticationService.SignIn((success, error) =>
            {
                if (success)
                {
                    PlayerIdentity identity = PlayerIdentityService.Current;
                    Session = identity == null ? null : new AuthenticationSession
                    {
                        UserId = identity.UserId,
                        DisplayName = identity.DisplayName,
                        ExpiresAtUtc = DateTime.MinValue
                    };
                    State = AuthenticationState.SignedIn;
                }
                else
                {
                    Session = null;
                    LastError = string.IsNullOrWhiteSpace(error) ? "Sign-in failed." : error;
                    State = AuthenticationState.Error;
                }
                Changed?.Invoke();
            });
        }

        public void SignOut()
        {
            if (State == AuthenticationState.SigningOut) return;

            State = AuthenticationState.SigningOut;
            Changed?.Invoke();

            AuthenticationService.SignOut(success =>
            {
                if (success)
                {
                    Session = null;
                    LastError = string.Empty;
                    State = AuthenticationState.SignedOut;
                }
                else
                {
                    LastError = "Sign-out failed.";
                    State = AuthenticationState.Error;
                }
                Changed?.Invoke();
            });
        }

        public void RestoreOfflineIdentity()
        {
            PlayerIdentityService.Initialize();
            Session = null;
            LastError = string.Empty;
            State = AuthenticationState.SignedOut;
            Changed?.Invoke();
        }
    }
}
