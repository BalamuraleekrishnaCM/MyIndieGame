using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class DodgeLineGame : ProductionMiniGameBase
    {
        public const string Id = "dodge-line";
        public override string GameId => Id;

        [SerializeField] int laneCount = 3;
        [SerializeField] int pointsPerDodge = 1;
        [SerializeField] int maxLives = 1;
        int lane;
        int lives;
        int dodges;

        public int Lane => lane;
        public int LaneCount => Mathf.Max(2, laneCount);
        public int Lives => lives;
        public int Dodges => dodges;

        protected override void ResetGame()
        {
            lane = LaneCount / 2;
            lives = Mathf.Max(1, maxLives);
            dodges = 0;
        }

        public void MoveLeft() => Move(-1);
        public void MoveRight() => Move(1);

        public void Move(int delta)
        {
            if (!IsRunning) return;
            lane = Mathf.Clamp(lane + delta, 0, LaneCount - 1);
        }

        public void DodgeObstacle(int obstacleLane)
        {
            if (!IsRunning) return;
            if (obstacleLane != lane)
            {
                dodges++;
                AddScore(Mathf.Max(0, pointsPerDodge));
            }
            else HitObstacle();
        }

        public void HitObstacle()
        {
            if (!IsRunning) return;
            lives = Mathf.Max(0, lives - 1);
            if (lives == 0) CompleteGame();
        }
    }
}
