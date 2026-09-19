# Complete Milestone Task Matrix

Repository implementation is tracked independently from Unity/device/store/backend validation.

| Version | Task area | Repository status | External validation |
|---|---|---|---|
| V1.10 | Game architecture | Implemented | Unity compile required |
| V1.11 | Tap Rush migration | Implemented | Play Mode/device required |
| V1.12 | Color Match, Stack It, Dodge Line | Implemented | Play Mode/device required |
| V1.13 | Coin Catch, Memory Flip, One Tap Jump | Implemented | Play Mode/device required |
| V1.14 | Ball Sort, Parking Puzzle, Merge 2048 | Implemented | Play Mode/device required |
| V1.15 | Production content boundaries | Implemented foundation | Scene/prefab/tuning in Unity |
| V1.16 | Safe area/accessibility/shared UX foundations | Implemented foundation | Device accessibility audit |
| V1.17 | Audio/haptics/game-feel orchestration | Implemented foundation | Device tuning |
| V1.18 | Progression/achievements | Existing services retained/integrated | Persistence regression |
| V1.19 | Auth/cloud/economy/leaderboards | Existing provider boundaries retained | Backend integration |
| V1.20 | Analytics/consent/remote config | Implemented foundation | Provider/privacy validation |
| V1.21 | Monetization | Implemented provider abstraction | Billing/ad sandbox + store policy |
| V1.22 | Performance | Implemented budget foundation | Unity Profiler/device thermal testing |
| V1.23 | QA/release candidate | Implemented release gates | Full device regression |
| V1.24 | Deployment | Release workflow documented | Signing/store/backend execution |
| V1.25 | Post-launch | Monitoring/hotfix structure documented | Live telemetry and release execution |

## Non-negotiable release gates

1. All 10 stable game IDs remain unchanged.
2. Unity project compiles without errors.
3. Every game passes Play Mode regression.
4. Android target-device smoke tests pass.
5. Backend production credentials/configuration are verified outside source control.
6. IAP/ad providers are tested in sandbox before release.
7. Privacy/consent behavior is reviewed for the distribution regions.
8. Release signing and store metadata are verified.
9. P0/P1 defects are closed before V1.23/V1.24 completion.
10. No source-only implementation is treated as device/store validation.
