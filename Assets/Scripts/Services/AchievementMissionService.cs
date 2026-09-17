using System;
using System.Collections.Generic;

namespace MyIndieGame.Services
{
    [Serializable]
    public sealed class AchievementDefinition
    {
        public string Id;
        public string StatisticKey;
        public long Target;
        public int RewardCoins;
    }

    [Serializable]
    public sealed class AchievementProgress
    {
        public string Id;
        public long Current;
        public long Target;
        public bool Completed;
        public bool RewardClaimed;
    }

    [Serializable]
    public sealed class MissionDefinition
    {
        public string Id;
        public string StatisticKey;
        public long Target;
        public int RewardCoins;
    }

    [Serializable]
    public sealed class MissionProgress
    {
        public string Id;
        public long Current;
        public long Target;
        public bool Completed;
        public bool RewardClaimed;
    }

    public interface IAchievementMissionService
    {
        void RegisterAchievement(AchievementDefinition definition);
        void RegisterMission(MissionDefinition definition);
        void RecordStatistic(string key, long delta);
        IReadOnlyList<AchievementProgress> GetAchievements();
        IReadOnlyList<MissionProgress> GetMissions();
        bool TryClaimAchievement(string id, out int rewardCoins, out string claimId);
        bool TryClaimMission(string id, out int rewardCoins, out string claimId);
    }

    /// <summary>
    /// Provider-neutral progression service. Reward claims are issued once locally
    /// and expose an idempotent claim ID for the V1.7 economy/server adapter.
    /// </summary>
    public sealed class AchievementMissionService : IAchievementMissionService
    {
        private readonly Dictionary<string, AchievementDefinition> achievements = new();
        private readonly Dictionary<string, AchievementProgress> achievementProgress = new();
        private readonly Dictionary<string, MissionDefinition> missions = new();
        private readonly Dictionary<string, MissionProgress> missionProgress = new();
        private readonly HashSet<string> claimed = new();
        private readonly Dictionary<string, long> statistics = new();

        public void RegisterAchievement(AchievementDefinition definition)
        {
            if (definition == null || string.IsNullOrWhiteSpace(definition.Id) || definition.Target <= 0)
                return;
            achievements[definition.Id] = definition;
            achievementProgress[definition.Id] = new AchievementProgress { Id = definition.Id, Target = definition.Target };
        }

        public void RegisterMission(MissionDefinition definition)
        {
            if (definition == null || string.IsNullOrWhiteSpace(definition.Id) || definition.Target <= 0)
                return;
            missions[definition.Id] = definition;
            missionProgress[definition.Id] = new MissionProgress { Id = definition.Id, Target = definition.Target };
        }

        public void RecordStatistic(string key, long delta)
        {
            if (string.IsNullOrWhiteSpace(key) || delta <= 0)
                return;
            statistics[key] = statistics.TryGetValue(key, out var value) ? value + delta : delta;
            RefreshProgress();
        }

        public IReadOnlyList<AchievementProgress> GetAchievements() => new List<AchievementProgress>(achievementProgress.Values);
        public IReadOnlyList<MissionProgress> GetMissions() => new List<MissionProgress>(missionProgress.Values);

        public bool TryClaimAchievement(string id, out int rewardCoins, out string claimId)
        {
            rewardCoins = 0;
            claimId = null;
            if (!achievementProgress.TryGetValue(id, out var progress) || !progress.Completed || progress.RewardClaimed)
                return false;
            var definition = achievements[id];
            claimId = "achievement:" + id;
            if (!claimed.Add(claimId)) return false;
            progress.RewardClaimed = true;
            rewardCoins = Math.Max(0, definition.RewardCoins);
            return true;
        }

        public bool TryClaimMission(string id, out int rewardCoins, out string claimId)
        {
            rewardCoins = 0;
            claimId = null;
            if (!missionProgress.TryGetValue(id, out var progress) || !progress.Completed || progress.RewardClaimed)
                return false;
            var definition = missions[id];
            claimId = "mission:" + id;
            if (!claimed.Add(claimId)) return false;
            progress.RewardClaimed = true;
            rewardCoins = Math.Max(0, definition.RewardCoins);
            return true;
        }

        private void RefreshProgress()
        {
            foreach (var pair in achievements)
            {
                var definition = pair.Value;
                var current = statistics.TryGetValue(definition.StatisticKey, out var value) ? value : 0;
                var progress = achievementProgress[pair.Key];
                progress.Current = Math.Min(current, definition.Target);
                progress.Completed = current >= definition.Target;
            }
            foreach (var pair in missions)
            {
                var definition = pair.Value;
                var current = statistics.TryGetValue(definition.StatisticKey, out var value) ? value : 0;
                var progress = missionProgress[pair.Key];
                progress.Current = Math.Min(current, definition.Target);
                progress.Completed = current >= definition.Target;
            }
        }
    }
}
