# QA Regression Matrix

## Core lifecycle

| Area | Cases |
|---|---|
| Launch | cold start, warm start, first-run state |
| Hub | all ten games visible, selection, return home |
| Session | start, duplicate start, pause, resume, end, complete |
| Results | score display, best score, repeat run, return home |
| Persistence | restart, clear data, malformed local values |
| Offline | launch offline, play offline, reconnect |

## Game matrix

For every game: start -> input -> score -> pause/resume -> completion -> results -> home -> replay. Repeat at least once after an application restart.

### Games
- [ ] Tap Rush
- [ ] Color Match
- [ ] Stack It
- [ ] Dodge Line
- [ ] Coin Catch
- [ ] Memory Flip
- [ ] One Tap Jump
- [ ] Ball Sort
- [ ] Parking Puzzle
- [ ] Merge 2048

## Android

- [ ] Touch input
- [ ] Back button behavior
- [ ] Screen rotation policy
- [ ] Different aspect ratios
- [ ] Low-memory/reload behavior
- [ ] 60 FPS target where supported
- [ ] No repeated audio/haptic systems after scene transitions
- [ ] No crashes during repeated game switching

## Network/backend

- [ ] Authentication expiry/recovery
- [ ] Cloud pull/push retry
- [ ] Conflict resolution
- [ ] Leaderboard retry/idempotency
- [ ] Economy transaction idempotency
- [ ] Server-side validation/RLS
- [ ] No privileged credentials in client

## Release evidence

Record Unity version, Android device/model, OS version, build number, commit SHA, test date, result, and reproduction details for every failure.
