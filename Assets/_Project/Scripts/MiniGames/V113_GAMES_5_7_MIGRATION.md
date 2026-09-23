# V1.13 — Games 5–7 Migration

Migrates Coin Catch, Memory Flip and One Tap Jump to IMiniGame/MiniGameSession.

## Stable IDs
- coin-catch
- memory-flip
- one-tap-jump

## Acceptance
- Dedicated adapters exist for all three games.
- Lifecycle and scoring use MiniGameSession.
- Completion uses the shared session boundary.
- Stable catalog identities remain unchanged.
- Existing prototype gameplay remains available during migration.
- Unity compilation, Play Mode and device validation remain required locally.
