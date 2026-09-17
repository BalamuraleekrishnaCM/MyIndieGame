using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class CoinCatchGame : ProductionMiniGameBase
    {
        public const string Id = "coin-catch";
        public override string GameId => Id;

        [SerializeField] int pointsPerCoin = 1;
        [SerializeField] int missPenalty = 0;
        int caught;

        public int Caught => caught;

        protected override void ResetGame()
        {
            caught = 0;
        }

        public void CatchCoin()
        {
            if (!IsRunning) return;
            caught++;
            AddScore(Mathf.Max(0, pointsPerCoin));
        }

        public void MissCoin()
        {
            if (!IsRunning || missPenalty <= 0) return;
            // Never allow a gameplay event to make the score negative.
            AddScore(0);
        }
    }
}
