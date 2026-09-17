# MyIndieGame — Master Roadmap

## Status

V1.0–V1.9 are complete in the current development history. V1.10 establishes the architecture foundation and is tracked separately through PR #21. This roadmap defines the remaining production path rather than treating the original prototype as the finished product.

## Milestones

| Milestone | Focus | Exit criteria |
|---|---|---|
| V1.10 | Game architecture foundation | Catalog, lifecycle contract, session boundary merged |
| V1.11 | First game migration | Tap Rush migrated to architecture, regression-safe |
| V1.12 | Games 2–4 migration | Color Match, Stack It, Dodge Line migrated |
| V1.13 | Games 5–7 migration | Coin Catch, Memory Flip, One Tap Jump migrated |
| V1.14 | Games 8–10 migration | Ball Sort, Parking Puzzle, Merge 2048 migrated |
| V1.15 | Production game content | Dedicated scenes/prefabs, tuned mechanics, level/config data |
| V1.16 | Shared UX and accessibility | Production Home/Game/Results UX, safe areas, accessibility pass |
| V1.17 | Audio, haptics and game feel | Shared feedback system integrated and tuned across all games |
| V1.18 | Progression and achievements | Progression loops, achievement definitions, UI and persistence |
| V1.19 | Backend integration hardening | Auth/cloud sync/economy/leaderboards integrated with robust error/offline handling |
| V1.20 | Analytics and live-ops foundation | Event schema, consent/privacy handling, diagnostics, remote-config-ready structure |
| V1.21 | Monetization | Ads/IAP architecture, reward flow, restore purchases, failure handling, store-policy readiness |
| V1.22 | Performance and memory | Mobile profiling, allocation reduction, load-time and battery/thermal pass |
| V1.23 | QA and release candidate | Full regression, device matrix, crash/error review, P0/P1 closure |
| V1.24 | Production deployment | Signed Android build, store metadata/assets, release tracks, backend production config |
| V1.25 | Post-launch hardening | Crash monitoring, hotfix process, analytics validation, first live update |

## Dependency order

`V1.10 → V1.11 → V1.12 → V1.13 → V1.14 → V1.15 → V1.16 → V1.17 → V1.18 → V1.19 → V1.20 → V1.21 → V1.22 → V1.23 → V1.24 → V1.25`

## Milestone rules

- Do not skip acceptance criteria to advance a version.
- Preserve all 10 stable game IDs.
- Do not remove working prototype functionality without an equivalent replacement.
- Keep server-authoritative economy and leaderboard boundaries intact.
- Every milestone must include documentation updates when architecture or operational behavior changes.
- Release milestones require Unity/device validation in addition to GitHub review.

## Definition of release

The product is considered ready for public deployment only after V1.24 acceptance criteria are met. V1.25 is the controlled post-launch stabilization phase.
