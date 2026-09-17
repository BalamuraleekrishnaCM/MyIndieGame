using System;

namespace MyIndieGame.Core
{
    [Serializable]
    public sealed class CloudPlayerData
    {
        public string UserId;
        public string DisplayName;
        public int Coins;
        public int GamesPlayed;
        public int TotalScore;
        public int Wins;
        public long UpdatedAtUnix;
    }
}
