using System;

namespace MyIndieGame.Core
{
    public interface IMiniGame
    {
        string GameId { get; }
        bool IsRunning { get; }
        int Score { get; }
        event Action<int> ScoreChanged;
        event Action<int> Completed;
        void StartGame();
        void PauseGame();
        void ResumeGame();
        void EndGame();
    }
}
