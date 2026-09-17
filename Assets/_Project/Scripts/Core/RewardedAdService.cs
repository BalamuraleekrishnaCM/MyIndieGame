using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    public sealed class RewardedAdService : MonoBehaviour
    {
        public static RewardedAdService Instance { get; private set; }
        public event Action<int> RewardGranted;

        const string RemoveAdsKey = "mig.store.remove_ads";

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public bool AdsRemoved => PlayerPrefs.GetInt(RemoveAdsKey, 0) == 1;

        public void MarkAdsRemoved()
        {
            PlayerPrefs.SetInt(RemoveAdsKey, 1);
            PlayerPrefs.Save();
        }

        public bool IsRewardedAdAvailable()
        {
            // Provider SDK integration is intentionally deferred to the release build.
            return false;
        }

        public bool ShowRewardedAd()
        {
            // Do not grant rewards here. The ad provider must invoke GrantReward only
            // after a verified completed reward callback.
            return false;
        }

        public void GrantReward(int coins)
        {
            if (coins <= 0 || GameSession.Instance == null) return;
            GameSession.Instance.AddCoins(coins);
            RewardGranted?.Invoke(coins);
        }
    }
}
