using System.Collections.Generic;

namespace MyIndieGame.Core
{
    public static class MiniGameCatalog
    {
        public static readonly IReadOnlyList<MiniGameDefinition> All = new[]
        {
            new MiniGameDefinition { Id = "tap_rush", Title = "Tap Rush", Description = "Hit the target as fast as possible.", SceneName = "TapRush" },
            new MiniGameDefinition { Id = "color_match", Title = "Color Match", Description = "Choose the matching color.", SceneName = "ColorMatch" },
            new MiniGameDefinition { Id = "stack_it", Title = "Stack It", Description = "Time your drops to build a tower.", SceneName = "StackIt" },
            new MiniGameDefinition { Id = "dodge_line", Title = "Dodge Line", Description = "Survive while obstacles approach.", SceneName = "DodgeLine" },
            new MiniGameDefinition { Id = "coin_catch", Title = "Coin Catch", Description = "Catch coins and avoid hazards.", SceneName = "CoinCatch" },
            new MiniGameDefinition { Id = "memory_flip", Title = "Memory Flip", Description = "Find every matching pair.", SceneName = "MemoryFlip" },
            new MiniGameDefinition { Id = "one_tap_jump", Title = "One Tap Jump", Description = "Jump over every obstacle.", SceneName = "OneTapJump" },
            new MiniGameDefinition { Id = "ball_sort", Title = "Ball Sort", Description = "Sort every color into its tube.", SceneName = "BallSort" },
            new MiniGameDefinition { Id = "parking_puzzle", Title = "Parking Puzzle", Description = "Clear the path and park the car.", SceneName = "ParkingPuzzle" },
            new MiniGameDefinition { Id = "merge_2048", Title = "Merge 2048", Description = "Merge tiles to reach the target.", SceneName = "Merge2048" }
        };
    }
}
