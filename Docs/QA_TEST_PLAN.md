# QA and Release Test Plan

## P0 Release Blocking
- App launches on target Android devices.
- All ten games open.
- All stable IDs remain unchanged.
- Every game starts and reaches a valid result.
- No crash in the core loop.
- Progress does not corrupt.

## P1
Pause/resume, back navigation, safe areas, large text, reduced motion, audio/haptics, offline/retry, leaderboard failures, economy recovery and IAP/ad sandbox behavior.

## Per-Game Regression
Fresh start → input → score change → completion/game over → results → replay → exit → reopen.

## Device Matrix
Low-, mid- and high-range Android devices; multiple aspect ratios; supported OS versions; representative refresh rates.

## Performance
Measure frame time, CPU/GPU load, memory, allocations and thermal behavior during representative sessions.

## Release Gate
Unity build verified, device smoke complete, crash review complete, backend production configured, privacy/consent ready, signed AAB verified, store metadata verified and all P0/P1 issues closed.
