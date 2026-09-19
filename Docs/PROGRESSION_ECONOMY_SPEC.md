# Progression, Economy and Rewards

## Local Progress
Per game: best score, play count and discovered state.

## Progression
Future systems may include achievements, daily missions, streaks, unlocks and cosmetics. Stable game IDs must never change.

## Economy
Virtual/premium currency is server-authoritative. Client UI may display cached values but cannot fabricate successful transactions.

## Rewards
Reward grants must have explicit verified paths and idempotency. Rewarded ads grant rewards only after provider verification.

## Anti-Abuse
Server validates player identity, product/reward identity, idempotency and transaction state. Never trust client currency balances.

## Analytics
Reward claims are consent-gated and should include source/context when permitted.
