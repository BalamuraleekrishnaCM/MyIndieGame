using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyIndieGame.Core
{
    [Serializable]
    public sealed class GameDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public int TimeLimitSeconds { get; }

        public GameDefinition(string id, string displayName, string description, int timeLimitSeconds)
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
            TimeLimitSeconds = Mathf.Max(1, timeLimitSeconds);
        }
    }

    public static class GameCatalog
    {
        static readonly GameDefinition[] Definitions =
        {
            new GameDefinition("tap-rush", "Tap Rush", "Tap the target", 30),
            new GameDefinition("color-match", "Color Match", "Match the color", 30),
            new GameDefinition("stack-it", "Stack It", "Build the stack", 30),
            new GameDefinition("dodge-line", "Dodge Line", "Survive the run", 30),
            new GameDefinition("coin-catch", "Coin Catch", "Catch the coins", 30),
            new GameDefinition("memory-flip", "Memory Flip", "Find every pair", 60),
            new GameDefinition("one-tap-jump", "One Tap Jump", "Tap to jump", 30),
            new GameDefinition("ball-sort", "Ball Sort", "Sort every tube", 60),
            new GameDefinition("parking-puzzle", "Parking Puzzle", "Clear the parking path", 60),
            new GameDefinition("merge-2048", "Merge 2048", "Merge the tiles", 60)
        };

        public static IReadOnlyList<GameDefinition> All => Definitions;

        public static bool TryGet(string id, out GameDefinition definition)
        {
            definition = null;
            if (string.IsNullOrWhiteSpace(id)) return false;
            for (int i = 0; i < Definitions.Length; i++)
            {
                if (string.Equals(Definitions[i].Id, id.Trim(), StringComparison.Ordinal))
                {
                    definition = Definitions[i];
                    return true;
                }
            }
            return false;
        }

        public static GameDefinition Get(int index)
        {
            return index >= 0 && index < Definitions.Length ? Definitions[index] : null;
        }
    }
}
