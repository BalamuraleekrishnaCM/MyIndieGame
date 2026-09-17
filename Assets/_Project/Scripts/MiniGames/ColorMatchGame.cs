using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class ColorMatchGame : ProductionMiniGameBase
    {
        public const string Id = "color-match";
        public override string GameId => Id;

        [SerializeField] int colorCount = 4;
        [SerializeField] int pointsPerCorrect = 2;
        [SerializeField] int wrongPenalty = 1;
        int targetColor;
        int rounds;

        public int TargetColor => targetColor;
        public int Rounds => rounds;
        public int ColorCount => Mathf.Max(2, colorCount);

        protected override void ResetGame()
        {
            targetColor = 0;
            rounds = 0;
        }

        protected override void OnGameStarted() => NextRound();

        public bool SelectColor(int colorIndex)
        {
            if (!IsRunning || colorIndex < 0 || colorIndex >= ColorCount) return false;
            bool correct = colorIndex == targetColor;
            rounds++;
            if (correct) AddScore(Mathf.Max(0, pointsPerCorrect));
            else if (wrongPenalty > 0) AddScore(-wrongPenalty);
            NextRound();
            return correct;
        }

        void NextRound() => targetColor = Random.Range(0, ColorCount);
    }
}
