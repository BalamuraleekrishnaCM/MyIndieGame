# MyIndieGame — Game Specifications

## Shared requirements

Every game uses its stable ID, `GameDefinition`, `IMiniGame`, and `MiniGameSession`. Each game supports Start, Pause/Resume where appropriate, Complete, Retry, Exit, score reporting, progress recording, feedback, analytics, and offline-safe behavior.

## 1. Tap Rush — `tap-rush`

- Objective: tap valid targets as quickly and accurately as possible.
- Core scoring: valid tap + combo multiplier; invalid/missed targets reduce efficiency.
- Session: 30 seconds.
- Difficulty: target size, spawn cadence, movement, and combo pressure increase by stage.
- UX: immediate hit feedback, countdown, score/combo HUD, clear end state.
- QA: missed taps, rapid taps, screen-edge targets, pause/background, score bounds.

## 2. Color Match — `color-match`

- Objective: select the matching color from presented choices.
- Core scoring: correct answer increases streak; incorrect answer breaks streak.
- Session: 30 seconds.
- Difficulty: more choices, shorter decision time, closer visual distractors.
- Accessibility: never rely on color alone; include labels/shapes/pattern cues.
- QA: color-blind simulation, rapid selection, duplicate-looking colors, timeout.

## 3. Stack It — `stack-it`

- Objective: align moving blocks to build a tall stack.
- Core scoring: overlap quality determines retained block width and points.
- Session: 30 seconds or failure.
- Difficulty: movement speed and precision demand increase progressively.
- QA: deterministic timing, missed placement, pause, restart, physics stability.

## 4. Dodge Line — `dodge-line`

- Objective: move through a constrained space while avoiding obstacles.
- Core scoring: survival time plus successful obstacle passes.
- Session: 30 seconds or collision.
- Difficulty: obstacle frequency/pattern complexity rises while preserving reaction time.
- QA: spawn fairness, collision boundaries, touch drag/tap behavior, backgrounding.

## 5. Coin Catch — `coin-catch`

- Objective: catch beneficial objects while avoiding hazards.
- Core scoring: object value and streak multiplier; hazards penalize or end the run.
- Session: 30 seconds.
- Difficulty: spawn density, movement speed, and hazard mix.
- QA: spawn overlap, edge cases, score integrity, rapid input, restart.

## 6. Memory Flip — `memory-flip`

- Objective: match every pair on the board.
- Core scoring: completion bonus minus move/time penalties.
- Session: up to 60 seconds.
- Difficulty: board size and memory pressure increase across levels.
- UX: readable card states, controlled flip timing, no accidental double input.
- QA: repeated taps, simultaneous inputs, last pair, timeout, restart.

## 7. One Tap Jump — `one-tap-jump`

- Objective: use one-touch jump timing to clear hazards.
- Core scoring: distance/pass count with milestone bonuses.
- Session: 30 seconds or collision.
- Difficulty: obstacle spacing and timing windows increase progressively.
- QA: tap buffering, landing/collision edge cases, pause/background, deterministic restart.

## 8. Ball Sort — `ball-sort`

- Objective: sort colored balls into matching containers.
- Core scoring: level completion, move efficiency, and optional time bonus.
- Session: up to 60 seconds per puzzle.
- Difficulty: container count and color count increase.
- UX: undo/restart where designed; invalid moves must be clearly communicated.
- QA: solvability, completion detection, undo state, restart, rapid selection.

## 9. Parking Puzzle — `parking-puzzle`

- Objective: move vehicles according to grid constraints to free the target vehicle.
- Core scoring: minimum/efficient moves plus completion bonus.
- Session: up to 60 seconds per puzzle.
- Difficulty: board complexity and move count increase.
- UX: clear grid, legal movement preview, restart, optional hint framework.
- QA: collision rules, blocked movement, completion detection, reset integrity.

## 10. Merge 2048 — `merge-2048`

- Objective: combine equal tiles to create larger values.
- Core scoring: standard merge-value scoring with overflow protection.
- Session: up to 60 seconds or board lock.
- Difficulty: score progression and optional challenge variants; core rules remain deterministic.
- UX: swipe controls, animation completion gating, clear game-over state.
- QA: all swipe directions, multiple merges, chained merges, full-board detection, score overflow, restart.

## Cross-game balance targets

- First successful interaction should occur within seconds.
- Sessions should be short enough for repeat play.
- Difficulty should increase without sudden unfair jumps.
- Scores must be deterministic for equivalent inputs where game rules allow it.
- Results must clearly explain score, best score, and available next actions.
