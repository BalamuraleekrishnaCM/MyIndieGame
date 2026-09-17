using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    /// <summary>
    /// Central navigation state for the game hub. UI can subscribe without
    /// knowing how scenes/panels are implemented, making later scene-based
    /// migration non-breaking.
    /// </summary>
    public static class GameNavigationService
    {
        public enum Screen
        {
            Home,
            Game,
            Results
        }

        public static Screen Current { get; private set; } = Screen.Home;
        public static int CurrentGameIndex { get; private set; } = -1;
        public static int LastScore { get; private set; }

        public static event Action<Screen> ScreenChanged;
        public static event Action<int> GameSelected;

        public static void GoHome()
        {
            CurrentGameIndex = -1;
            LastScore = 0;
            SetScreen(Screen.Home);
        }

        public static bool OpenGame(int gameIndex)
        {
            if (gameIndex < 0) return false;
            CurrentGameIndex = gameIndex;
            LastScore = 0;
            GameProgressService.MarkDiscovered(GameId(gameIndex));
            SetScreen(Screen.Game);
            GameSelected?.Invoke(gameIndex);
            return true;
        }

        public static void ShowResults(int score)
        {
            if (CurrentGameIndex < 0) return;
            LastScore = Mathf.Max(0, score);
            GameProgressService.RecordGameFinished(GameId(CurrentGameIndex), LastScore);
            SetScreen(Screen.Results);
        }

        public static string GameId(int gameIndex)
        {
            switch (gameIndex)
            {
                case 0: return "tap-rush";
                case 1: return "color-match";
                case 2: return "stack-it";
                case 3: return "dodge-line";
                case 4: return "coin-catch";
                case 5: return "memory-flip";
                case 6: return "one-tap-jump";
                case 7: return "ball-sort";
                case 8: return "parking-puzzle";
                case 9: return "merge-2048";
                default: return string.Empty;
            }
        }

        static void SetScreen(Screen screen)
        {
            Current = screen;
            ScreenChanged?.Invoke(screen);
        }
    }
}
