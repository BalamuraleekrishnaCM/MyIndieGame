# MyIndieGame — Complete Game Design Document

## 1. Product Vision
MyIndieGame is a casual mini-game hub built around fast, readable, replayable experiences. A player should reach gameplay within seconds, understand the mechanic immediately, finish a short run, see a clear result, and replay to improve.

## 2. Product Goals
- Ten distinct mini-games with stable IDs.
- Consistent Home → Game → Results flow.
- Short sessions, simple controls, strong game feel.
- Local-first gameplay with explicit online-service boundaries.
- Android-first production and release.
- Accessibility, safe-area and performance foundations shared across games.

## 3. Target Audience
Casual mobile players seeking short sessions, quick challenges and score improvement. The design should remain understandable without specialist gaming knowledge.

## 4. Game Line-up
| ID | Game | Core loop |
|---|---|---|
| tap-rush | Tap Rush | Tap targets rapidly for score |
| color-match | Color Match | Match the requested color |
| stack-it | Stack It | Place blocks accurately |
| dodge-line | Dodge Line | Avoid hazards and survive |
| coin-catch | Coin Catch | Catch valuable objects |
| memory-flip | Memory Flip | Find matching pairs |
| one-tap-jump | One Tap Jump | Time jumps over obstacles |
| ball-sort | Ball Sort | Sort balls into matching containers |
| parking-puzzle | Parking Puzzle | Solve vehicle movement puzzles |
| merge-2048 | Merge 2048 | Merge equal tiles |

Stable IDs are immutable contracts used by progression, analytics and backend systems.

## 5. Core Player Loop
Home → select game → start → play → complete/game over → results → replay/next/home.

Opening a game marks discovery. Completion records local progress and exposes the final score through the shared session boundary.

## 6. Game Design Principles
1. One primary mechanic per game.
2. First interaction should teach the mechanic.
3. Input must feel immediate.
4. Score feedback must be visible.
5. Failure must be understandable.
6. Runs should be short enough to invite replay.
7. Difficulty should rise through predictable tuning.
8. Do not use color alone to communicate critical state.
9. Accessibility preferences must be respected.
10. Online failures must not falsely appear successful.

## 7. Game Architecture
Every game implements IMiniGame. MiniGameSession owns common lifecycle and score state. GameCatalog resolves stable definitions. Game-specific mechanics remain isolated.

Lifecycle: StartGame → gameplay → PauseGame/ResumeGame when applicable → EndGame or completion.

## 8. Progression
Per-game local state:
- best score
- play count
- discovered state

Future systems may add achievements, missions, streaks, unlocks and cosmetics without changing stable IDs.

## 9. Accessibility
Shared support includes safe-area handling, reduced motion and large text. Games must maintain readable contrast, usable touch targets and non-color-only state communication.

## 10. Game Feel
Success, failure, click, score and completion feedback should be consistent through shared orchestration. Audio, haptics and nonessential motion must respect user preferences.

## 11. Analytics
Analytics is consent-gated. Standard events:
- game_started
- game_completed
- reward_claimed

Include stable game ID and useful session/score metadata where privacy rules allow.

## 12. Monetization
Architecture supports IAP, remove-ads and coin products plus a verified rewarded-ad boundary. Real store SDK behavior and sandbox validation remain release activities.

## 13. Online Services
Authentication, cloud sync, server-authoritative economy and leaderboards remain explicit service boundaries. Client-local progress must never be treated as authoritative currency or transaction state.

## 14. Performance
Project guardrail: 60 FPS target and a tracked-object budget of 2500. Avoid hot-loop allocations and repeated persistence writes. Profile on representative Android hardware.

## 15. Release
Development → staging → production. Release requires Unity build verification, device smoke testing, crash review, production backend configuration, privacy/consent readiness, signed AAB and store metadata.

## 16. Definition of Done
A game is production-ready when its core loop, lifecycle, scoring, results, replay, accessibility, feedback, analytics, performance and device QA have been verified without changing its stable ID.
