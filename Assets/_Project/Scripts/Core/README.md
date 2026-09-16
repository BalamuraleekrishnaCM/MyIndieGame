# Core Systems

Reusable systems for the mini-game collection.

- `GameSession` — persistent player-level coins and play count.
- `BestScoreStore` — persistent best score per game.
- `GameFeel` — lightweight punch, haptic and camera-shake effects.
- `MiniGameDefinition` / `MiniGameCatalog` — single source of truth for the 10 games.
- `SessionProgress` — shared score/session lifecycle.
- `MiniGameRunner` — shared timed game lifecycle.
- `TouchInput` / `HapticFeedback` — mobile-friendly input and feedback helpers.

Keep individual game rules inside their own game modules. Core systems should remain game-agnostic.
