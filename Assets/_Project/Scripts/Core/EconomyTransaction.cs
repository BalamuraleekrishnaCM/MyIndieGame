using System;

namespace MyIndieGame.Core
{
    [Serializable]
    public sealed class EconomyTransaction
    {
        public string TransactionId;
        public string UserId;
        public string Reason;
        public int Delta;
        public int BalanceAfter;
        public long ClientCreatedAtUnix;
        public long ServerCreatedAtUnix;
        public long ServerVersion;
    }
}
