# Ten-Game Production Core Status

## Implemented runtime cores

| # | Game | Runtime core | Primary interaction | Completion condition |
|---|---|---|---|---|
| 1 | Tap Rush | `TapRushGame` | Tap target | Timer/session completion |
| 2 | Color Match | `ColorMatchGame` | Select color | Timer/session completion |
| 3 | Stack It | `StackItGame` | Drop aligned block | Target tower or miss |
| 4 | Dodge Line | `DodgeLineGame` | Change lane / dodge | Lives exhausted or timer |
| 5 | Coin Catch | `CoinCatchGame` | Catch coin | Timer/session completion |
| 6 | Memory Flip | `MemoryFlipGame` | Select cards | All pairs matched or timer |
| 7 | One Tap Jump | `OneTapJumpGame` | Jump / clear obstacle | Lives exhausted or timer |
| 8 | Ball Sort | `BallSortGame` | Move balls between tubes | All color tubes solved |
| 9 | Parking Puzzle | `ParkingPuzzleGame` | Move car / clear exit | Car reaches open exit |
| 10 | Merge 2048 | `Merge2048Game` | Swipe/arrow-equivalent move | Target tile reached or timer |

## Shared contract

All production cores derive from `ProductionMiniGameBase` and therefore expose the shared `IMiniGame` lifecycle, score state, timer, pause/resume and completion event. The base routes score state through `MiniGameSession`.

## Integration requirements

The runtime cores are gameplay/domain controllers. Scene composition, visual prefabs, animations, audio, haptics, touch gesture binding and accessibility remain presentation/integration work and must be validated in Unity Play Mode and on Android devices.

## Validation boundary

GitHub source inspection can verify the presence and static structure of the ten cores. It cannot establish Unity compilation, Play Mode behavior, frame pacing, touch behavior, Android installation, or store readiness without the appropriate build/device environment.
