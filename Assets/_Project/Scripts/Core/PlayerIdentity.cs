using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    [Serializable]
    public sealed class PlayerIdentity
    {
        public string UserId;
        public string DisplayName;
        public bool IsAuthenticated;
    }

    public static class PlayerIdentityService
    {
        const string UserIdKey = "mig.identity.user_id";
        const string DisplayNameKey = "mig.identity.display_name";

        public static PlayerIdentity Current { get; private set; }
        public static event Action Changed;

        public static void Initialize()
        {
            Current = new PlayerIdentity
            {
                UserId = PlayerPrefs.GetString(UserIdKey, string.Empty),
                DisplayName = PlayerPrefs.GetString(DisplayNameKey, "Player"),
                IsAuthenticated = false
            };
            Changed?.Invoke();
        }

        public static void SetAuthenticated(string userId, string displayName)
        {
            if (string.IsNullOrWhiteSpace(userId)) return;
            Current = new PlayerIdentity
            {
                UserId = userId.Trim(),
                DisplayName = string.IsNullOrWhiteSpace(displayName) ? "Player" : displayName.Trim(),
                IsAuthenticated = true
            };
            PlayerPrefs.SetString(UserIdKey, Current.UserId);
            PlayerPrefs.SetString(DisplayNameKey, Current.DisplayName);
            PlayerPrefs.Save();
            Changed?.Invoke();
        }

        public static void SignOut()
        {
            Current = new PlayerIdentity { UserId = string.Empty, DisplayName = "Player", IsAuthenticated = false };
            PlayerPrefs.DeleteKey(UserIdKey);
            PlayerPrefs.DeleteKey(DisplayNameKey);
            PlayerPrefs.Save();
            Changed?.Invoke();
        }
    }
}
