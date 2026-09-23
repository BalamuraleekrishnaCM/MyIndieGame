# V1.12 — Games 2–4 Migration

## Scope
Migrate Color Match, Stack It, and Dodge Line to the shared mini-game architecture introduced in V1.10 and established by Tap Rush in V1.11.

## Stable identities
- Color Match: `color-match` (catalog index 1)
- Stack It: `stack-it` (catalog index 2)
- Dodge Line: `dodge-line` (catalog index 3)

## Architecture requirements
- Each game implements `IMiniGame`.
- Each game owns a `MiniGameSession` created from `GameCatalog`.
- Start, pause, resume, end, score, and completion flow through the session boundary.
- Gameplay input systems call game-specific scoring methods rather than writing progress directly.
- Completion routes through `MiniGameSession.Complete()`, preserving shared navigation and progress behavior.
- Existing `MiniGameCollection` prototype code remains available during incremental migration.

## Game adapters
| Game | Gameplay boundary | Default session index |
|---|---|---:|
| Color Match | `SubmitMatch(bool matched, int points)` | 1 |
| Stack It | `PlaceBlock(bool successful, int points)` | 2 |
| Dodge Line | `RecordDodge(int points)` | 3 |

## Acceptance criteria
- [x] Dedicated architecture class exists for all three games.
- [x] Stable game IDs are preserved.
- [x] Catalog definitions are resolved by stable ID.
- [x] Lifecycle delegates to `MiniGameSession`.
- [x] Score changes delegate to `MiniGameSession.AddScore`.
- [x] Completion delegates to `MiniGameSession.Complete`.
- [ ] Unity Editor compilation verification.
- [ ] Play Mode gameplay verification.
- [ ] Android/device smoke test.

## Validation note
GitHub validation confirms the source structure and migration files only. Unity compilation, Play Mode, and device validation must be performed in the Unity project environment.
