# MyIndieGame — Complete Game Production Plan

## 1. Product goal

MyIndieGame is a mobile-first casual-game hub containing 10 polished mini-games. The product should ship as one cohesive application with shared navigation, progression, audio, haptics, achievements, analytics, monetization, online services, and production-quality mobile performance.

## 2. Production principles

- Preserve the existing Unity project structure and stable game IDs.
- Keep each mini-game independently testable and replaceable.
- Use shared systems rather than duplicating navigation, progress, audio, haptics, analytics, economy, and services.
- Local progress is convenience data only; server-authoritative systems remain authoritative.
- Avoid pay-to-win mechanics. Monetization should not invalidate competitive integrity.
- Design for touch first, then accommodate other input where practical.
- Every milestone must leave `develop` in a coherent, reviewable state.

## 3. Game catalogue and production targets

| ID | Game | Core loop | Target session | Primary production work |
|---|---|---|---|---|
| tap-rush | Tap Rush | Tap targets rapidly before time expires | 30s | Input feel, combo/scoring, timer, feedback, difficulty curve |
| color-match | Color Match | Match the requested color under time pressure | 30s | Rules, readability, speed ramp, accessibility |
| stack-it | Stack It | Time moving blocks to build the tallest stack | 30s | Physics/timing, camera, fail state, score curve |
| dodge-line | Dodge Line | Avoid incoming obstacles for as long as possible | 30s | Movement, obstacle patterns, fairness, difficulty |
| coin-catch | Coin Catch | Catch valuable objects while avoiding misses/hazards | 30s | Spawn system, scoring, combo, feedback |
| memory-flip | Memory Flip | Find matching card pairs | 60s | Board generation, flip animation, difficulty tiers |
| one-tap-jump | One Tap Jump | Tap to jump through hazards | 30s | Physics, timing windows, obstacle generation |
| ball-sort | Ball Sort | Sort colored balls into matching containers | 60s | Puzzle rules, validation, undo/restart, level data |
| parking-puzzle | Parking Puzzle | Solve vehicle movement/parking puzzles | 60s | Grid rules, level data, hints, completion validation |
| merge-2048 | Merge 2048 | Merge equal tiles to reach higher values | 60s | Grid logic, swipe input, deterministic scoring, progression |

## 4. Definition of done for every game

A game is production-ready only when all of the following are complete:

1. Implements `IMiniGame` and uses `MiniGameSession`.
2. Has a dedicated game controller/prefab or scene boundary.
3. Has deterministic start, pause, resume, completion, retry, and exit behavior.
4. Has a clear scoring model and score limits validated for overflow/invalid input.
5. Has a difficulty curve and at least three meaningful difficulty stages or equivalent progression.
6. Has touch input with safe-area support.
7. Has visual, audio, and haptic feedback where appropriate.
8. Has accessibility/readability checks for text, contrast, and color-dependent rules.
9. Records local progress through the shared progress system.
10. Emits analytics events through the shared analytics layer without leaking sensitive data.
11. Handles offline operation gracefully.
12. Has automated/unit tests for core rules where practical.
13. Has a manual QA checklist covering normal, edge, interruption, and restart flows.
14. Has no known P0/P1 gameplay defects before release candidate.

## 5. Shared game architecture

### Core layers

- `GameDefinition` / `GameCatalog`: identity and metadata.
- `IMiniGame`: lifecycle contract.
- `MiniGameSession`: standard session state and integration.
- `GameNavigationService`: Home → Game → Results flow.
- `GameProgressService`: local best scores, plays, discovery.
- `AudioFeedback` / `HapticFeedback` / `GameFeel`: common feedback.
- `AchievementService`: achievement evaluation and presentation.
- `AnalyticsService`: telemetry abstraction.
- `EconomyService`: server-authoritative currency/reward contracts.
- `LeaderboardService`: server-backed competitive scores.
- Authentication/cloud services: account identity and cloud synchronization.

### Game data

Prefer data-driven level/configuration assets for values that need tuning. Gameplay code should not hard-code large level catalogues or balance tables.

## 6. UX flow

Home → game selection → game intro/countdown → gameplay → pause (if applicable) → result → rewards/achievement/leaderboard summary → retry or next game → Home.

All exits must safely persist local progress and leave shared services in a valid state.

## 7. QA matrix

Test every game on supported target Android devices and representative performance tiers.

### Functional
- First launch
- First game
- Repeat game
- Pause/resume
- App background/foreground
- Incoming interruption
- Rotation/configuration behavior as applicable
- Restart
- Exit to Home
- Offline mode
- Reconnection
- Invalid/maximum scores
- Corrupted/missing local data

### Performance
- Stable target frame rate
- No sustained main-thread spikes during normal play
- No runaway allocations during gameplay loops
- No texture/audio memory regressions
- Fast scene/prefab startup
- Thermal/battery sanity check on long sessions

### Accessibility
- Text legibility
- Color-blind-safe cues for Color Match and other color-based mechanics
- Touch target sizing
- Haptic/audio alternatives where important
- Reduced-motion considerations where practical

## 8. Release quality gates

### P0 — blocker
Crash, data corruption, broken startup, unrecoverable progression/economy/leaderboard integrity, or severe input failure.

### P1 — release-critical
Major game flow failure, persistent soft lock, severe performance regression, broken monetization transaction, or broken account/cloud synchronization.

### P2 — normal release backlog
Cosmetic issues, minor balancing problems, non-blocking UI defects.

No P0/P1 issues are allowed for a production release candidate.

## 9. Milestone execution model

Each milestone follows:

1. Inspect current `develop`.
2. Create `feature/vX-Y-*` from `develop`.
3. Implement and document.
4. Static/code review.
5. Unity editor compilation and Play Mode validation locally.
6. Device QA where applicable.
7. Open PR to `develop`.
8. Resolve review issues.
9. Merge PR.
10. Verify `develop` after merge.
11. Start the next milestone only after verification.

## 10. Scope sequence

The detailed milestone schedule is defined in `Docs/ROADMAP.md`. Game-specific acceptance criteria are in `Docs/GAME_SPECIFICATIONS.md`. Deployment and store-release gates are in `Docs/DEPLOYMENT_PLAN.md`.
