using UnityEngine;

namespace MyIndieGame.MiniGames
{
    public sealed class Merge2048Game : ProductionMiniGameBase
    {
        public const string Id = "merge-2048";
        public override string GameId => Id;

        [SerializeField] int width = 4;
        [SerializeField] int height = 4;
        [SerializeField] int winValue = 2048;
        readonly int[] cells = new int[16];
        bool moved;

        public int Width => 4;
        public int Height => 4;
        public int GetCell(int x, int y) => x < 0 || x >= Width || y < 0 || y >= Height ? 0 : cells[y * Width + x];
        public bool LastMoveChanged => moved;

        protected override void ResetGame()
        {
            for (int i = 0; i < cells.Length; i++) cells[i] = 0;
            moved = false;
        }

        protected override void OnGameStarted()
        {
            SpawnTile();
            SpawnTile();
        }

        public bool MoveLeft() => Move(0);
        public bool MoveUp() => Move(1);
        public bool MoveRight() => Move(2);
        public bool MoveDown() => Move(3);

        public bool Move(int direction)
        {
            if (!IsRunning || direction < 0 || direction > 3) return false;
            moved = false;
            for (int line = 0; line < Width; line++)
            {
                int[] values = new int[Width];
                for (int i = 0; i < Width; i++)
                {
                    int x = direction == 0 ? i : direction == 2 ? Width - 1 - i : line;
                    int y = direction == 1 ? i : direction == 3 ? Height - 1 - i : line;
                    if (direction <= 2 && direction != 1 && direction != 3) { if (direction == 0 || direction == 2) y = line; }
                    if (direction == 1 || direction == 3) x = line;
                    values[i] = GetCell(x, y);
                }
                int[] compact = new int[Width];
                int write = 0;
                for (int i = 0; i < Width; i++) if (values[i] != 0) compact[write++] = values[i];
                for (int i = 0; i < Width - 1; i++)
                {
                    if (compact[i] == 0 || compact[i] != compact[i + 1]) continue;
                    compact[i] *= 2;
                    compact[i + 1] = 0;
                    AddScore(compact[i]);
                }
                write = 0;
                for (int i = 0; i < Width; i++) if (compact[i] != 0) compact[write++] = compact[i];
                while (write < Width) compact[write++] = 0;
                for (int i = 0; i < Width; i++)
                {
                    int x = direction == 0 ? i : direction == 2 ? Width - 1 - i : line;
                    int y = direction == 1 ? i : direction == 3 ? Height - 1 - i : line;
                    if (direction == 1 || direction == 3) x = line;
                    int index = y * Width + x;
                    if (cells[index] != compact[i]) moved = true;
                    cells[index] = compact[i];
                }
            }
            if (!moved) return false;
            if (HasValue(winValue)) CompleteGame();
            else SpawnTile();
            return true;
        }

        bool HasValue(int value)
        {
            for (int i = 0; i < cells.Length; i++) if (cells[i] >= value) return true;
            return false;
        }

        void SpawnTile()
        {
            int emptyCount = 0;
            for (int i = 0; i < cells.Length; i++) if (cells[i] == 0) emptyCount++;
            if (emptyCount == 0) return;
            int pick = Random.Range(0, emptyCount);
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] != 0) continue;
                if (pick-- > 0) continue;
                cells[i] = Random.value < 0.9f ? 2 : 4;
                return;
            }
        }
    }
}
