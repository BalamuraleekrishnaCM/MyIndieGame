using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    [Serializable]
    public sealed class MiniGameDefinition
    {
        public string id;
        public string title;
        public string description;
        public int targetScore;
        public int rewardCoins;

        public MiniGameDefinition(string id, string title, string description, int targetScore, int rewardCoins)
        {
            this.id = id; this.title = title; this.description = description;
            this.targetScore = targetScore; this.rewardCoins = rewardCoins;
        }
    }

    public static class MiniGameCatalog
    {
        public static readonly MiniGameDefinition[] All =
        {
            new("tap_rush", "Tap Rush", "Hit the target as fast as possible.", 20, 5),
            new("color_match", "Color Match", "Choose the matching color.", 20, 5),
            new("stack_it", "Stack It", "Time your drops to build a tower.", 10, 6),
            new("dodge_line", "Dodge Line", "Survive while obstacles approach.", 20, 6),
            new("coin_catch", "Coin Catch", "Catch coins and avoid hazards.", 15, 6),
            new("memory_flip", "Memory Flip", "Find every matching pair.", 8, 8),
            new("one_tap_jump", "One Tap Jump", "Jump over every obstacle.", 15, 6),
            new("ball_sort", "Ball Sort", "Sort every color into its tube.", 12, 8),
            new("parking_puzzle", "Parking Puzzle", "Clear the path and park the car.", 5, 8),
            new("merge_2048", "Merge 2048", "Merge tiles to reach the target.", 16, 10)
        };
    }
}
