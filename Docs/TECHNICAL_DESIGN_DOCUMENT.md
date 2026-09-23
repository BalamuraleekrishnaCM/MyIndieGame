# Technical Design Document

## Architecture
Presentation/UI → Game Adapter → IMiniGame → MiniGameSession → Navigation/Progression/Services.

Game-specific mechanics stay isolated. Shared systems provide lifecycle, scoring, persistence, analytics, feedback, configuration and monetization boundaries.

## Core Contracts
- IMiniGame: game ID, running state, score and lifecycle events.
- MiniGameSession: active/paused state, score mutation and completion.
- GameCatalog: stable game definition lookup.
- GameNavigationService: Home/Game/Results transitions.
- GameProgressService: local best, play count and discovery.
- AnalyticsService: consent-gated event recording.
- RemoteConfigService: typed, default-safe configuration.
- MonetizationService: provider abstraction.
- FeedbackOrchestrator: shared feedback routing.
- AccessibilitySettings: persistent accessibility preferences.
- SafeAreaController: safe-area layout.
- PerformanceBudget: runtime guardrails.
- ReleaseChecklist: release gates.

## Data Ownership
Game adapters own transient mechanic state. MiniGameSession owns score/lifecycle. Progression owns lightweight local progress. Backend services own authoritative economy/online state. Analytics owns event emission.

## Persistence
PlayerPrefs is suitable for lightweight local state and settings. It is not a secure store and must not be treated as server authority.

## Failure Handling
Invalid IDs are rejected. Score arithmetic is bounded. Provider failures remain explicit. Offline/local gameplay must not claim a server transaction succeeded.

## Scene and Prefab Strategy
Give each game an isolated scene/prefab boundary while reusing global UI, feedback and service infrastructure. Avoid duplicate global service implementations.

## Testing
Unit: lifecycle, score bounds, catalog IDs, progress rules, consent, remote-config defaults and monetization provider behavior.

Play Mode: game start/pause/resume/end/complete, navigation, input, UI, feedback and results.

Device: safe areas, Android lifecycle, back navigation, frame time, memory, thermal behavior, audio/haptics and representative hardware.

Release: signed AAB, upgrade path, offline/online transitions, IAP/ad sandbox, privacy and store compliance.

## Security
Never embed secrets in the client. Server endpoints must validate identity, game ID, score and idempotency. Treat client score/progression as untrusted when syncing.

## Performance
Avoid per-frame allocations, excessive Instantiate/Destroy, repeated component lookups, unbounded particles/audio and frequent PlayerPrefs writes. Profile before optimization.

## Observability
Track consent-compliant game starts/completions, service failures, purchases, ads, crashes and unusual score/reward patterns.
