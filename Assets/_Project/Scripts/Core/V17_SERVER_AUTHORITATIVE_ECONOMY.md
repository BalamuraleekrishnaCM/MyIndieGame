# V1.7 Server-Authoritative Economy

V1.7 establishes the client-side boundary required to move coins from local mutation to a server-authoritative economy.

## Implemented
- `EconomyTransaction` carries transaction identity, reason, delta, balance and server metadata.
- `EconomyService` exposes provider-neutral balance, reward and spend operations.
- Explicit economy sync states distinguish unavailable, queued, syncing, ready and error states.
- `EconomyTransactionGuard` provides short-lived client idempotency protection and retry-safe transaction IDs.

## Authority model
The Unity client must never be the source of truth for coin balances once a production provider is connected.

Production flow:

`Authenticated user -> EconomyService -> Provider Adapter -> Server/API -> Transaction Ledger -> Authoritative Balance`

The server must calculate or validate reward amounts, authenticate the user, enforce ownership, and atomically write the transaction and resulting balance.

## Required server controls
- Server-issued authenticated subject; never trust a client-supplied user ID.
- Atomic balance updates and transaction ledger entries.
- Durable unique idempotency key per transaction.
- Reject negative/invalid reward requests and impossible state transitions.
- Server-side validation of game results, mission claims and achievement rewards.
- Replay protection and rate limiting.
- Audit timestamps and monotonic server versioning.
- RLS/authorization rules for user-owned records where applicable.
- No service-role or privileged backend secret in the Unity client.

## Reward integration boundary
Existing local reward producers (`SessionProgress`, daily rewards, missions and future purchases) must migrate to `EconomyService.ApplyReward()` through provider adapters. Until a production backend is connected, local `GameSession.AddCoins()` remains legacy prototype behavior and must not be presented as authoritative online economy.

## Offline policy
Offline rewards should be represented as pending intents only. A production implementation must reconcile them against server validation and idempotency rules; it must not blindly upload a client-chosen balance.

## Production gates
- Select and authorize a backend provider.
- Implement server transaction endpoint/function and durable ledger.
- Integrate authentication tokens/session expiry.
- Implement server-side reward validation.
- Add duplicate-request, retry, rollback/compensation and offline/reconnect tests.
- Migrate all reward sources away from direct client coin mutation.
- Add observability for rejected, duplicated and suspicious transactions.
- Verify Unity/package compatibility and run device QA.

This milestone intentionally contains no live backend connection, real-money transaction path, service-role credential, or claim of successful Unity/device testing.
