# M7-M13 Parallel Execution

## Objective
Drive the remaining production work as parallel workstreams while keeping `develop` as the integration branch and `main` release-only.

## Milestones

| Milestone | Workstream | Repository deliverable | Runtime gate |
|---|---|---|---|
| M7 | Production games | Dedicated game contracts, migration matrix, production acceptance criteria | Unity Play Mode + Android |
| M8 | UI/UX + feedback | Shared UX/feedback contracts and accessibility checklist | Device visual/touch QA |
| M9 | Persistence/progression | Local persistence boundaries and recovery tests | Restart/offline tests |
| M10 | Android build | Deterministic build configuration and CI validation | Signed Android build |
| M11 | Optimization | Performance budget and profiling checklist | Device profiling |
| M12 | Deployment | Release runbook, versioning, CI/CD and rollback | Store/track deployment |
| M13 | Final QA/release | Release candidate gate and regression matrix | Full device acceptance |

## Ten-game production matrix

1. Tap Rush
2. Color Match
3. Stack It
4. Dodge Line
5. Coin Catch
6. Memory Flip
7. One Tap Jump
8. Ball Sort
9. Parking Puzzle
10. Merge 2048

Each game must satisfy: deterministic lifecycle, score ownership through the shared session, safe restart/end, pause/resume, touch input, no unbounded allocations in the hot loop, local best-score persistence, and a documented failure/recovery path.

## Shared quality gates

- No secrets, signing keys, service-role credentials, or production tokens in Git.
- Local progress is never treated as authoritative for economy, entitlements, or online ranking.
- Game IDs remain stable across migrations.
- Every gameplay completion is idempotent.
- All ten games are reachable from the hub.
- Application launch, game start, pause, resume, completion, results, and return-home transitions are covered by regression checks.
- Android back handling and interrupted-session recovery are defined.

## Execution rule
Implementation may proceed in parallel, but promotion to `main` requires all repository checks to pass and the manual Unity/Android gates below to be explicitly recorded. GitHub-side work must not claim that a Unity compiler, physical device, store console, or live backend was executed when it was not.
