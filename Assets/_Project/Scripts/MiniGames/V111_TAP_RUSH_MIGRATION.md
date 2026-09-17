# V1.11 — Tap Rush Migration

## Goal

Move Tap Rush behind the V1.10 game architecture boundary without rewriting the existing prototype controller in one step.

## Architecture

`TapRushGame` implements `IMiniGame` and owns a `MiniGameSession` created from the stable `tap-rush` catalog definition.

The existing prototype may remain in place during this incremental migration. Future work should move Tap Rush input, timer, target spawning, combo logic, and presentation into the dedicated game boundary while keeping shared navigation/progress in Core.

## Integration contract

- Stable ID: `tap-rush`
- Catalog index: `0`
- Session target: 30 seconds
- Score changes flow through `MiniGameSession`.
- Completion flows through `GameNavigationService` and local `GameProgressService` via the session.
- No client-authoritative economy or leaderboard rewards are introduced.

## Acceptance criteria

- [x] Dedicated `TapRushGame` architecture adapter exists.
- [x] Uses `IMiniGame`.
- [x] Uses `MiniGameSession`.
- [x] Preserves stable game ID.
- [x] Provides lifecycle methods.
- [x] Provides score input boundary.
- [x] Provides completion boundary.
- [ ] Unity compile validation.
- [ ] Play Mode regression validation.
- [ ] Android device validation.
- [ ] Full Tap Rush mechanic extraction from the legacy monolith.

The remaining unchecked items are deliberate release/validation work and are not claimed as complete by the GitHub-only implementation.
