# Release Runbook

## Versioning
- Keep the release version in one authoritative project setting/constant.
- Use semantic versioning for public releases.
- Create a release branch from `develop` only after the RC gate passes.

## Pre-release
- Confirm all 10 games are present in the catalog and hub.
- Run repository validation workflow.
- Open Unity in the project version declared by `ProjectSettings/ProjectVersion.txt`.
- Run compile, Play Mode smoke tests, and full regression matrix.
- Build Development and Release Android variants.
- Test clean install, upgrade install, restart, offline launch, offline/online transition, back navigation, pause/resume, and interrupted game recovery.
- Profile startup, hub navigation and representative gameplay loops on target Android hardware.

## Signing
Never commit keystores, passwords, service accounts, signing certificates containing private material, or store credentials. Configure them as CI secrets/secure files outside Git.

## Release candidate
1. Freeze gameplay changes.
2. Record commit SHA and application version.
3. Produce signed AAB/APK through the controlled build environment.
4. Smoke test the artifact on physical devices.
5. Record known issues and rollback version.

## Deployment
- Upload to internal testing first.
- Promote to closed testing after smoke/regression acceptance.
- Promote to production only after release approval and store checks.
- Keep the previous known-good artifact available for rollback.

## Rollback
- Stop further promotion.
- Identify the affected version and commit.
- Restore the last known-good release artifact/configuration.
- Record incident, impact, and corrective action.
- Re-run smoke tests before resuming promotion.

## Post-release
Monitor crash rate, startup failures, ANR, gameplay completion failures, authentication/cloud-sync failures, economy/leaderboard anomalies, and store feedback. Do not collect analytics beyond the configured privacy/consent policy.
