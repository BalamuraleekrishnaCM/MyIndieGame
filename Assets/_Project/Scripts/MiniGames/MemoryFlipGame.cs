using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class MemoryFlipGame : ProductionMiniGameBase
    {
        public const string Id = "memory-flip";
        public override string GameId => Id;

        [SerializeField] int pairCount = 6;
        [SerializeField] int pointsPerPair = 10;
        [SerializeField] int mismatchPenalty = 0;

        readonly HashSet<int> matchedPairs = new HashSet<int>();
        int firstCard = -1;
        int matches;

        public int Matches => matches;
        public int PairCount => Mathf.Max(2, pairCount);
        public bool HasFirstCard => firstCard >= 0;

        protected override void ResetGame()
        {
            matchedPairs.Clear();
            firstCard = -1;
            matches = 0;
        }

        public bool SelectCard(int cardIndex, int pairId)
        {
            if (!IsRunning || cardIndex < 0 || matchedPairs.Contains(pairId)) return false;

            if (firstCard < 0)
            {
                firstCard = cardIndex;
                return true;
            }

            if (cardIndex == firstCard) return false;

            bool matched = pairId == PairIdForCard(firstCard);
            if (matched)
            {
                matchedPairs.Add(pairId);
                matches++;
                AddScore(Mathf.Max(0, pointsPerPair));
                if (matches >= PairCount) CompleteGame();
            }
            else if (mismatchPenalty > 0)
            {
                // Score remains non-negative; penalties are represented by zero-point mismatch events.
                AddScore(0);
            }

            firstCard = -1;
            return matched;
        }

        // UI/controllers should supply a stable pair mapping. The default mapping is deterministic.
        static int PairIdForCard(int cardIndex) => Math.Max(0, cardIndex / 2);
    }
}
