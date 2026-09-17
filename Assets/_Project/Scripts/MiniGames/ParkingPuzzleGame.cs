using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class ParkingPuzzleGame : ProductionMiniGameBase
    {
        public const string Id = "parking-puzzle";
        public override string GameId => Id;

        [SerializeField] int gridWidth = 5;
        [SerializeField] int gridHeight = 4;
        [SerializeField] int targetMoves = 20;
        [SerializeField] int pointsForExit = 100;
        int carX;
        int carY;
        int moves;
        bool exitOpen;

        public int CarX => carX;
        public int CarY => carY;
        public int Moves => moves;
        public bool ExitOpen => exitOpen;
        public int GridWidth => Mathf.Max(3, gridWidth);
        public int GridHeight => Mathf.Max(3, gridHeight);

        protected override void ResetGame()
        {
            carX = 1;
            carY = GridHeight / 2;
            moves = 0;
            exitOpen = false;
        }

        public bool MoveCar(int dx, int dy)
        {
            if (!IsRunning) return false;
            int nx = Mathf.Clamp(carX + Mathf.Clamp(dx, -1, 1), 0, GridWidth - 1);
            int ny = Mathf.Clamp(carY + Mathf.Clamp(dy, -1, 1), 0, GridHeight - 1);
            if (nx == carX && ny == carY) return false;
            carX = nx;
            carY = ny;
            moves++;
            exitOpen = moves >= Mathf.Max(1, targetMoves);
            return true;
        }

        public bool ClearObstacle()
        {
            if (!IsRunning) return false;
            exitOpen = true;
            return true;
        }

        public bool Exit()
        {
            if (!IsRunning || !exitOpen || carX != GridWidth - 1) return false;
            AddScore(Mathf.Max(0, pointsForExit));
            CompleteGame();
            return true;
        }
    }
}
