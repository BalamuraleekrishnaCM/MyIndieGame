# M7–M13 Release Execution

This document tracks the remaining production milestones after V1.7 hardening.

## M7 — Production game migration

### Repository implementation
- Shared `ProductionMiniGameBase` provides a common IMiniGame lifecycle, score events and timer.
- Coin Catch, Memory Flip and One Tap Jump now have dedicated production runtime cores.
- Existing game IDs remain stable through `GameCatalog`.

### Acceptance
- [x] Shared runtime base committed
- [x] Games 5–7 production cores committed
- [ ] Unity compilation in the project's configured Unity version
- [ ] Play Mode verification
- [ ] Touch/device verification
- [ ] All 10 games migrated and visually integrated

## M8 — UI/UX, audio and game feel

- Production UI must use safe-area aware layouts and scalable touch targets.
- Audio/haptics must honor existing settings services.
- Add game-specific feedback without coupling games to platform SDKs.
- Validate portrait/landscape policy, device aspect ratios and accessibility text sizes.

Acceptance: Unity/device validation required.

## M9 — Progression and online systems

- Preserve local-first progress and existing server-authority boundaries.
- Connect achievements/missions to gameplay statistics.
- Connect leaderboard submission only after authenticated backend validation.
- Economy rewards must remain server-authoritative.

Acceptance: backend integration, RLS, retry/idempotency and offline/online tests required.

## M10 — Android build and device validation

- Configure Android application identity and release build settings.
- Produce development and release APK/AAB builds.
- Test fresh install, upgrade, uninstall/reinstall, offline mode, low-memory conditions and suspend/resume.
- Verify touch, frame pacing, audio, haptics, safe area and persistence on representative Android devices.

Acceptance: physical device build/test required.

## M11 — Optimization and release hardening

- Profile CPU, GPU, memory allocations and loading time.
- Remove avoidable per-frame allocations.
- Verify no duplicate persistent systems after scene transitions.
- Validate lifecycle behavior under pause/resume, backgrounding and repeated game launches.
- Run final regression across all games.

Acceptance: Unity profiler + device regression required.

## M12 — Deployment and store readiness

- Configure CI build pipeline and artifact retention.
- Configure signing outside source control; never commit keystores/passwords/secrets.
- Prepare release notes, privacy disclosures, support information and store assets.
- Establish internal/closed testing track and rollback procedure.

Acceptance: successful signed AAB and store-console validation required.

## M13 — Final QA and release

Release gate requires:

1. Clean checkout opens without missing packages.
2. Unity compilation succeeds with zero errors.
3. All 10 games launch, play and complete correctly.
4. Persistence survives restart.
5. Network failures degrade safely.
6. Economy and leaderboard authority is server-side.
7. No secrets are present in the client repository.
8. Release AAB installs and upgrades correctly.
9. Performance and memory meet the project's device targets.
10. Final release candidate is tagged and documented.

## Validation boundary

GitHub repository operations can implement source and documentation changes, but they do not provide the Unity Editor, Android SDK/device runtime, store console, or production signing environment. Those gates must be executed in the project's build environment before claiming a production release.
