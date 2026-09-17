# V1.9 Local Progress & Navigation Foundation

## Scope
V1.9 establishes provider-neutral local progression and a central navigation state model for the 10-game hub. It deliberately does not move gameplay into separate scenes yet; the current prototype can adopt these contracts incrementally.

## Local progress
`GameProgressService` persists only client-owned convenience data:
- personal best score per game
- play count per game
- discovered/unlocked-in-hub state

Data is stored through Unity `PlayerPrefs`. It is not authoritative for economy, online ranking, rewards, or anti-cheat decisions.

Stable game IDs are:
- `tap-rush`
- `color-match`
- `stack-it`
- `dodge-line`
- `coin-catch`
- `memory-flip`
- `one-tap-jump`
- `ball-sort`
- `parking-puzzle`
- `merge-2048`

## Navigation
`GameNavigationService` exposes three states:
- `Home`
- `Game`
- `Results`

It also tracks the selected game and final score and emits events so UI/presentation code can subscribe without depending on a concrete scene implementation.

## Integration contract
When the prototype controller is migrated, call:
```csharp
GameNavigationService.OpenGame(gameIndex);
GameNavigationService.ShowResults(score);
GameNavigationService.GoHome();
```

For screens that need local progress:
```csharp
int best = GameProgressService.GetBest("tap-rush");
int plays = GameProgressService.GetPlayCount("tap-rush");
bool discovered = GameProgressService.IsDiscovered("tap-rush");
```

## Security boundary
Local progress is UX state only. Never use `GameProgressService` as proof of currency, purchases, online rank, premium entitlement, or server rewards. V1.7 economy and V1.8 leaderboard server-authority rules remain unchanged.

## Validation / release gates
- Static review of the new C# contracts.
- Unity compilation and Android runtime testing are still required in the local Unity environment.
- Verify persistence after app restart and after clearing PlayerPrefs.
- Verify navigation event ordering during Home -> Game -> Results -> Home.
- Verify leaderboard/economy code does not treat local progress as authoritative.
