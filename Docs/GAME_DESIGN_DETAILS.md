# Detailed Game Design Specifications

## Common Game Contract
Each game provides a clear start state, active state, completion/game-over state, score, result presentation, replay and exit. Gameplay score flows through MiniGameSession.

## 1. Tap Rush
**ID:** tap-rush  
**Objective:** Tap valid targets rapidly and maximize score.  
**Core mechanic:** Targets spawn into the play area; valid taps score points.  
**End:** Timer expires.  
**Tuning:** run duration, target lifetime, spawn interval, target size and intensity curve.  
**Feel:** fast spawn, immediate tap response, escalating tempo.

## 2. Color Match
**ID:** color-match  
**Objective:** Select the requested color before time expires.  
**Core mechanic:** Present a target and multiple choices.  
**End:** timeout or configured mistake condition.  
**Tuning:** choice count, response window, distractors and difficulty curve.  
**Accessibility:** never depend only on hue; provide labels/shapes where appropriate.

## 3. Stack It
**ID:** stack-it  
**Objective:** Build the tallest accurate stack.  
**Core mechanic:** Align and place a moving block over the existing stack.  
**End:** missed placement or configured run limit.  
**Tuning:** horizontal speed, block width, tolerance and camera progression.  
**Scoring:** successful placements, with optional accuracy bonuses.

## 4. Dodge Line
**ID:** dodge-line  
**Objective:** Survive while avoiding hazards.  
**Core mechanic:** Move within a bounded field/lane while hazards approach.  
**End:** collision or survival target.  
**Tuning:** hazard frequency, speed, safe-zone width and difficulty curve.  
**Scoring:** successful dodges and/or survival duration.

## 5. Coin Catch
**ID:** coin-catch  
**Objective:** Catch valuable objects and maximize score.  
**Core mechanic:** Move the catcher and collect falling/appearing coins.  
**End:** timer expiry.  
**Tuning:** spawn rate, object speed, values and special-object frequency.  
**Scoring:** object value, with optional streak bonuses.

## 6. Memory Flip
**ID:** memory-flip  
**Objective:** Match all pairs efficiently.  
**Core mechanic:** Flip two cards, compare them, retain matches and reset mismatches.  
**End:** all pairs found or configured move/time limit.  
**Tuning:** board size, reveal duration, move limit and time limit.  
**Scoring:** pairs plus efficiency bonuses.

## 7. One Tap Jump
**ID:** one-tap-jump  
**Objective:** Clear obstacles using one-button timing.  
**Core mechanic:** Tap triggers a jump while forward movement is automatic.  
**End:** collision or fall.  
**Tuning:** jump impulse, gravity, obstacle spacing and scroll speed.  
**Scoring:** obstacles cleared.

## 8. Ball Sort
**ID:** ball-sort  
**Objective:** Sort balls into matching containers.  
**Core mechanic:** Select a source and legal destination while obeying capacity/rules.  
**End:** puzzle completion; optional move limit.  
**Tuning:** colors, tubes, capacity and puzzle depth.  
**Scoring:** completion plus move/time efficiency.

## 9. Parking Puzzle
**ID:** parking-puzzle  
**Objective:** Solve a vehicle layout and clear the target path.  
**Core mechanic:** Move vehicles along permitted axes and spaces.  
**End:** target vehicle reaches the goal.  
**Tuning:** board size, vehicle count and solution depth.  
**Scoring:** completion plus move/time efficiency.

## 10. Merge 2048
**ID:** merge-2048  
**Objective:** Merge equal tiles and maximize score/best tile.  
**Core mechanic:** Directional movement combines equal adjacent tiles once per move using standard 2048 ordering.  
**End:** no legal moves remain.  
**Tuning:** board size, spawn distribution and target tile.  
**Scoring:** sum of merge values.

## Difficulty Model
Use three broad bands: onboarding, mastery and challenge. Difficulty should rise through measurable variables rather than arbitrary score inflation. Remote configuration may tune validated parameters without changing IDs or contracts.

## Results Standard
Every game should show final score, best score, improvement/delta, replay, next-game and home actions. The result state should clearly explain why the run ended.
