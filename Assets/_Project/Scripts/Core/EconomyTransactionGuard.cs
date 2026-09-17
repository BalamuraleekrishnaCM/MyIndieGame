using System;
using System.Collections.Generic;

namespace MyIndieGame.Core
{
    public static class EconomyTransactionGuard
    {
        const int MaxEntries = 128;
        static readonly HashSet<string> Submitted = new HashSet<string>();
        static readonly Queue<string> Order = new Queue<string>();

        public static string CreateId(string reason)
        {
            return string.Concat(reason ?? "reward", "_", Guid.NewGuid().ToString("N"));
        }

        public static bool TryBegin(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId) || Submitted.Contains(transactionId)) return false;
            Submitted.Add(transactionId);
            Order.Enqueue(transactionId);
            while (Order.Count > MaxEntries)
                Submitted.Remove(Order.Dequeue());
            return true;
        }

        public static void Forget(string transactionId)
        {
            // A failed request may be retried. The provider remains responsible
            // for durable idempotency once a transaction reaches the server.
            if (!string.IsNullOrWhiteSpace(transactionId)) Submitted.Remove(transactionId);
        }
    }
}
