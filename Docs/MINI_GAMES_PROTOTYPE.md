# MyIndieGame — Mini Games Prototype

## Product concept

MyIndieGame is a lightweight mobile casual-game hub. The main screen is a game library containing short, pick-up-and-play games. Each mini game should be understandable in seconds and playable in roughly 30 seconds to 3 minutes.

## Prototype goals

- Build one reusable hub instead of a single endless game.
- Start with 10 mini games.
- Keep each game mechanically simple.
- Reuse common UI, audio, scoring, pause, results, and navigation systems.
- Make every mini game independently testable.
- Design for portrait mobile first.
- Avoid unnecessary online/backend dependencies in V0.1.

## First 10 mini games

| # | Game | Core mechanic | Target session |
|---|---|---|---|
| 01 | Tap Rush | Tap targets as quickly as possible before time expires. | 30–60 sec |
| 02 | Color Match | Tap the object whose color matches the target. | 30–60 sec |
| 03 | Stack It | Tap at the right moment to stack moving blocks. | 1–2 min |
| 04 | Dodge Line | Move left/right to avoid incoming obstacles. | 1–3 min |
| 05 | Coin Catch | Move a basket/player and catch falling coins while avoiding bombs. | 1–2 min |
| 06 | Memory Flip | Flip cards and match pairs with as few moves as possible. | 1–3 min |
| 07 | One Tap Jump | Tap to jump over obstacles in a short endless-run loop. | 1–3 min |
| 08 | Ball Sort | Sort colored balls into matching containers. | 1–3 min |
| 09 | Parking Puzzle | Slide/drive vehicles to clear a path for the target vehicle. | 1–3 min |
| 10 | Merge 2048 | Swipe tiles to merge equal numbers and increase the score. | 1–5 min |

## Main hub flow

```text
Boot
  ↓
Main Menu / Game Library
  ↓
Select Mini Game
  ↓
Game Intro (optional, first play only)
  ↓
Gameplay
  ↓
Result Screen
  ├── Retry
  ├── Next Game
  └── Back to Game Library
```

## Shared systems

### GameHub
- Displays the 10 game cards.
- Shows locked/unlocked state if progression is enabled later.
- Opens the selected game scene.
- Stores simple best scores locally.

### GameSession
Common contract for every mini game:

- `StartGame()`
- `PauseGame()`
- `ResumeGame()`
- `EndGame()`
- `RestartGame()`
- `GetScore()`

### Results
Every game returns:

- Score
- Best score
- Coins/reward (optional)
- Retry
- Next game
- Home

## Unity structure

```text
Assets/
├── _Project/
│   ├── Art/
│   ├── Audio/
│   ├── Materials/
│   ├── Prefabs/
│   │   ├── Common/
│   │   └── MiniGames/
│   ├── Scenes/
│   │   ├── Boot.unity
│   │   ├── GameHub.unity
│   │   └── MiniGames/
│   │       ├── TapRush.unity
│   │       ├── ColorMatch.unity
│   │       ├── StackIt.unity
│   │       ├── DodgeLine.unity
│   │       ├── CoinCatch.unity
│   │       ├── MemoryFlip.unity
│   │       ├── OneTapJump.unity
│   │       ├── BallSort.unity
│   │       ├── ParkingPuzzle.unity
│   │       └── Merge2048.unity
│   ├── Scripts/
│   │   ├── Core/
│   │   ├── Hub/
│   │   ├── Common/
│   │   └── MiniGames/
│   │       ├── TapRush/
│   │       ├── ColorMatch/
│   │       ├── StackIt/
│   │       ├── DodgeLine/
│   │       ├── CoinCatch/
│   │       ├── MemoryFlip/
│   │       ├── OneTapJump/
│   │       ├── BallSort/
│   │       ├── ParkingPuzzle/
│   │       └── Merge2048/
│   ├── UI/
│   └── Settings/
├── Scenes/              # optional project-level scenes if needed
├── Packages/
└── ProjectSettings/
```

## Prototype implementation order

1. Shared Boot + GameHub.
2. Shared result/pause UI.
3. Implement Tap Rush.
4. Implement Color Match.
5. Implement Stack It.
6. Implement Dodge Line.
7. Implement Coin Catch.
8. Implement Memory Flip.
9. Implement One Tap Jump.
10. Implement Ball Sort.
11. Implement Parking Puzzle.
12. Implement Merge 2048.
13. Add local best-score persistence.
14. Mobile input and performance pass.

## V0.1 constraints

- Offline-first.
- No accounts.
- No ads required for the prototype.
- No multiplayer.
- No backend.
- Minimal art: primitives, gradients, icons, and simple animation are acceptable.
- All games should launch from the same hub and return cleanly to it.

## Future expansion

The hub is intentionally data-driven so additional mini games can be added without redesigning the main application. Future versions can add daily challenges, achievements, themes, coins, cloud saves, ads/IAP, leaderboards, and seasonal content.
