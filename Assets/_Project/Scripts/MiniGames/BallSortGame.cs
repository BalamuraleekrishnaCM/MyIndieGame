using System.Collections.Generic;
using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class BallSortGame : ProductionMiniGameBase
    {
        public const string Id = "ball-sort";
        public override string GameId => Id;

        [SerializeField] int tubeCount = 4;
        [SerializeField] int colors = 3;
        [SerializeField] int capacity = 4;
        [SerializeField] int pointsPerSolvedTube = 10;
        readonly List<List<int>> tubes = new List<List<int>>();
        int selectedTube = -1;
        int solvedTubes;

        public int TubeCount => Mathf.Max(3, tubeCount);
        public int ColorCount => Mathf.Max(2, colors);
        public int Capacity => Mathf.Max(2, capacity);
        public int SelectedTube => selectedTube;
        public int SolvedTubes => solvedTubes;
        public IReadOnlyList<List<int>> Tubes => tubes;

        protected override void ResetGame()
        {
            tubes.Clear();
            for (int i = 0; i < TubeCount; i++) tubes.Add(new List<int>(Capacity));
            for (int color = 0; color < ColorCount; color++)
                for (int n = 0; n < Capacity; n++) tubes[color].Add(color);
            selectedTube = -1;
            solvedTubes = 0;
        }

        public bool SelectTube(int index)
        {
            if (!IsRunning || index < 0 || index >= tubes.Count) return false;
            if (selectedTube < 0)
            {
                if (tubes[index].Count == 0) return false;
                selectedTube = index;
                return true;
            }
            if (selectedTube == index) { selectedTube = -1; return false; }
            bool moved = Move(selectedTube, index);
            selectedTube = -1;
            return moved;
        }

        public bool Move(int from, int to)
        {
            if (!IsRunning || from < 0 || to < 0 || from >= tubes.Count || to >= tubes.Count || from == to) return false;
            var source = tubes[from];
            var target = tubes[to];
            if (source.Count == 0 || target.Count >= Capacity) return false;
            int color = source[source.Count - 1];
            if (target.Count > 0 && target[target.Count - 1] != color) return false;
            target.Add(color);
            source.RemoveAt(source.Count - 1);
            CheckSolved(to);
            return true;
        }

        void CheckSolved(int index)
        {
            if (tubes[index].Count != Capacity) return;
            int color = tubes[index][0];
            for (int i = 1; i < Capacity; i++) if (tubes[index][i] != color) return;
            solvedTubes++;
            AddScore(Mathf.Max(0, pointsPerSolvedTube));
            if (solvedTubes >= ColorCount) CompleteGame();
        }
    }
}
