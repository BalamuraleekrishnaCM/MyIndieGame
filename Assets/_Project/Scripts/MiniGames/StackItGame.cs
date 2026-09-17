using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class StackItGame : ProductionMiniGameBase
    {
        public const string Id = "stack-it";
        public override string GameId => Id;

        [SerializeField] float alignmentTolerance = 0.18f;
        [SerializeField] int pointsPerBlock = 1;
        [SerializeField] int targetBlocks = 12;
        float currentX;
        float lastX;
        int blocks;
        int direction = 1;

        public float CurrentX => currentX;
        public float LastX => lastX;
        public int Blocks => blocks;
        public int TargetBlocks => Mathf.Max(1, targetBlocks);

        protected override void ResetGame()
        {
            currentX = 0.5f;
            lastX = 0.5f;
            blocks = 0;
            direction = 1;
        }

        protected override void OnGameStarted() => currentX = 0.25f;

        protected override void Tick(float remainingSeconds)
        {
            currentX += direction * Time.unscaledDeltaTime * 0.55f;
            if (currentX >= 0.85f) { currentX = 0.85f; direction = -1; }
            if (currentX <= 0.15f) { currentX = 0.15f; direction = 1; }
        }

        public bool DropBlock()
        {
            if (!IsRunning) return false;
            if (Mathf.Abs(currentX - lastX) > Mathf.Max(0.01f, alignmentTolerance))
            {
                CompleteGame();
                return false;
            }
            lastX = currentX;
            blocks++;
            AddScore(Mathf.Max(0, pointsPerBlock));
            if (blocks >= TargetBlocks) CompleteGame();
            return true;
        }
    }
}
