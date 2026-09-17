using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    public static class ChallengeService
    {
        const string ChallengePrefix = "mig.challenge.";

        public static bool IsCompleted(string challengeId)
        {
            return PlayerPrefs.GetInt(ChallengePrefix + challengeId, 0) == 1;
        }

        public static bool Complete(string challengeId)
        {
            if (string.IsNullOrWhiteSpace(challengeId) || IsCompleted(challengeId)) return false;
            PlayerPrefs.SetInt(ChallengePrefix + challengeId, 1);
            PlayerPrefs.Save();
            AnalyticsService.Record("challenge_completed", new System.Collections.Generic.Dictionary<string, object> { ["challenge_id"] = challengeId });
            return true;
        }
    }
}
