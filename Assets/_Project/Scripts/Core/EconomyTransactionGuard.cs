using System;
using System.Collections.Generic;

namespace MyIndieGame.Core
{
    public static class EconomyTransactionGuard
    {
        const int MaxEntries = 128;
        static readonly HashSet<string> Submitted = new HashSet<string>(StringComparer.Ordinal);
        static readonly Queue<string> Order = new Queue<string>();

        public static string CreateId(string reason)
        {
            string prefix = string.IsNullOrWhiteSpace(reason) ? "transaction" : reason.Trim();
            return string.Concat(prefix, "_", Guid.NewGuid().ToString("N"));
        }

        public static bool TryBegin(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId)) return false;
            transactionId = transactionId.Trim();
            if (Submitted.Contains(transactionId)) return false;
            Submitted.Add(transactionId);
            Order.Enqueue(transactionId);
            while (Order.Count > MaxEntries)
                Submitted.Remove(Order.Dequeue());
            return true;
        }

        public static void Forget(string transactionId)
        {
            // Only forget before the provider has accepted a transaction. Once a
            // request may have reached a server, retries must reuse the same ID.
            // The durable server ledger is the final idempotency authority.
            if (!string.IsNullOrWhiteSpace(transactionId)) Submitted.Remove(transactionId.Trim());
        }

        public static void ResetForTests()
        {
            Submitted.Clear();
            Order.Clear();
        }
    }
}
