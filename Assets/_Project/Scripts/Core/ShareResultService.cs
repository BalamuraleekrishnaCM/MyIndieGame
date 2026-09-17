using System;

namespace MyIndieGame.Core
{
    public static class ShareResultService
    {
        public static event Action<string> ShareRequested;

        public static string BuildMessage(string gameTitle, int score, int bestScore)
        {
            return $"I scored {score} in {gameTitle}! Best: {bestScore}. #MyIndieGame";
        }

        public static void RequestShare(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            ShareRequested?.Invoke(message);
        }
    }
}
