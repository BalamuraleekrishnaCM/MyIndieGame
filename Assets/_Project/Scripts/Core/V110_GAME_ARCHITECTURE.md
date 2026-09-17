# V1.10 — Game Architecture

## Goal
Establish a stable catalog and lifecycle boundary so individual mini-games can be moved out of the prototype controller incrementally without changing player-facing behavior.

## Contracts
- `GameCatalog` is the single source of truth for the current 10 game IDs, names, descriptions, and time limits.
- `IMiniGame` defines the common runtime lifecycle and score/completion events for future component-based games.
- `MiniGameSession` coordinates a game definition with `GameNavigationService` and `GameProgressService`.

## Migration rule
Existing `MiniGameCollection` remains intact in V1.10. New games should implement `IMiniGame` and use `MiniGameSession` rather than adding more game-specific state to the monolithic controller. Existing games can be migrated one at a time in later milestones.

## Authority boundary
Local progress remains convenience state. Economy and online leaderboard operations remain governed by their existing server-authoritative contracts from V1.7 and V1.8.

## Validation
Repository/static review only. Unity compilation and play-mode tests must be run in the project Unity version before release.
