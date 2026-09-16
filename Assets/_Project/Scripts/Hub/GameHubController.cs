using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyIndieGame.Core;

namespace MyIndieGame.Hub
{
    public class GameHubController : MonoBehaviour
    {
        [SerializeField] private List<MiniGameDefinition> games = new List<MiniGameDefinition>
        {
            new MiniGameDefinition { Id = "tap-rush", Title = "Tap Rush", Description = "Tap targets before time runs out.", SceneName = "TapRush" },
            new MiniGameDefinition { Id = "color-match", Title = "Color Match", Description = "Match the target color quickly.", SceneName = "ColorMatch" },
            new MiniGameDefinition { Id = "stack-it", Title = "Stack It", Description = "Time your taps to build the tallest stack.", SceneName = "StackIt" },
            new MiniGameDefinition { Id = "dodge-line", Title = "Dodge Line", Description = "Move and survive the incoming obstacles.", SceneName = "DodgeLine" },
            new MiniGameDefinition { Id = "coin-catch", Title = "Coin Catch", Description = "Catch coins and avoid hazards.", SceneName = "CoinCatch" },
            new MiniGameDefinition { Id = "memory-flip", Title = "Memory Flip", Description = "Find all matching card pairs.", SceneName = "MemoryFlip" },
            new MiniGameDefinition { Id = "one-tap-jump", Title = "One Tap Jump", Description = "Jump over obstacles with one tap.", SceneName = "OneTapJump" },
            new MiniGameDefinition { Id = "ball-sort", Title = "Ball Sort", Description = "Sort every color into the correct container.", SceneName = "BallSort" },
            new MiniGameDefinition { Id = "parking-puzzle", Title = "Parking Puzzle", Description = "Clear the path and move the target car.", SceneName = "ParkingPuzzle" },
            new MiniGameDefinition { Id = "merge-2048", Title = "Merge 2048", Description = "Merge matching tiles to grow your score.", SceneName = "Merge2048" }
        };

        public IReadOnlyList<MiniGameDefinition> Games => games;

        public void OpenGame(int index)
        {
            if (index < 0 || index >= games.Count) return;
            if (string.IsNullOrWhiteSpace(games[index].SceneName)) return;

            SceneManager.LoadScene(games[index].SceneName);
        }

        public void OpenGame(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName)) return;
            SceneManager.LoadScene(sceneName);
        }
    }
}
