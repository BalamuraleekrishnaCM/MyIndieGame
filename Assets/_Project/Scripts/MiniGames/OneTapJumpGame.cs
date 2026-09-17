using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class OneTapJumpGame : ProductionMiniGameBase
    {
        public const string Id = "one-tap-jump";
        public override string GameId => Id;

        [SerializeField] int pointsPerObstacle = 1;
        [SerializeField] int maxLives = 3;

        int lives;
        int obstaclesCleared;

        public int Lives => lives;
        public int ObstaclesCleared => obstaclesCleared;

        protected override void ResetGame()
        {
            lives = Mathf.Max(1, maxLives);
            obstaclesCleared = 0;
        }

        public void Jump()
        {
            if (!IsRunning) return;
            OnJumpRequested();
        }

        public void ClearObstacle()
        {
            if (!IsRunning) return;
            obstaclesCleared++;
            AddScore(Mathf.Max(0, pointsPerObstacle));
        }

        public void HitObstacle()
        {
            if (!IsRunning) return;
            lives = Mathf.Max(0, lives - 1);
            if (lives == 0) CompleteGame();
        }

        protected virtual void OnJumpRequested() { }
    }
}
