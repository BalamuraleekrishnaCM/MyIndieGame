# V1.14 — Games 8–10 Migration

Migrates Ball Sort, Parking Puzzle and Merge 2048 to IMiniGame/MiniGameSession.

## Stable IDs
- ball-sort
- parking-puzzle
- merge-2048

## Acceptance
- Dedicated adapters exist for all three games.
- Lifecycle and scoring use MiniGameSession.
- Completion uses the shared session boundary.
- Stable catalog identities remain unchanged.
- Existing prototype gameplay remains available during migration.
- Unity compilation, Play Mode and device validation remain required locally.
