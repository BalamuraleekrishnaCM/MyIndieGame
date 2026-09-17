# MyIndieGame — Deployment & Release Plan

## Target

Primary production target: Android mobile release. The deployment process must support internal testing, closed testing, production rollout, and controlled post-launch updates.

## 1. Build configuration

- Development: local debugging, development services, verbose diagnostics.
- Staging: release-like build against staging backend/services.
- Production: release signing, production service endpoints, production analytics, production economy/leaderboard configuration.
- Never commit signing keys, passwords, service secrets, or production credentials.

## 2. Android release gates

Before any release candidate:

- Unity project opens and compiles without project errors.
- All 10 games launch, play, complete, retry, and return to Home.
- Navigation and progress survive app restart.
- Authentication/cloud/economy/leaderboard flows behave correctly when online.
- Offline behavior is intentional and tested.
- Analytics events are verified in staging.
- Ads/IAP flows are tested with test products/accounts.
- No P0/P1 defects.
- Performance and memory checks pass on representative low/mid/high Android devices.
- Application ID, version name, version code, icons, splash, orientation, permissions, and target SDK are reviewed.

## 3. Signing

Use a dedicated release keystore stored outside source control or in a secure CI secret store. Back up the keystore and recovery information securely. Never place passwords or keystore files in the repository.

## 4. Build artifact

Preferred production artifact: Android App Bundle (`.aab`). Generate a signed release bundle and retain the exact source commit, Unity version, build configuration, version code, and signing identity metadata for reproducibility.

## 5. Testing tracks

1. Local development build.
2. Internal test track.
3. Closed test track.
4. Production staged rollout.
5. Full production availability.

Promotion requires the previous track's acceptance criteria to pass.

## 6. Store readiness

Prepare:

- App name and short/full descriptions.
- App icon and feature graphics.
- Phone screenshots and any required tablet/other form-factor assets.
- Privacy policy URL and data-safety declarations.
- Content rating questionnaire.
- Target audience declarations.
- Ads declaration.
- IAP product definitions and pricing.
- Support/contact information.
- Store category/tags.
- Release notes.

All declarations must match actual runtime behavior and collected data.

## 7. Backend production checklist

- Production database/schema applied and verified.
- Authentication configuration verified.
- Leaderboard endpoints protected and rate-limited appropriately.
- Economy operations remain server-authoritative.
- Idempotency handling verified for reward/score submissions.
- Cloud-save conflict behavior tested.
- Monitoring/logging enabled without collecting unnecessary personal data.
- Staging and production credentials/endpoints separated.

## 8. CI/CD direction

CI should eventually perform:

1. Checkout exact commit.
2. Validate repository structure.
3. Restore dependencies.
4. Run available static/unit tests.
5. Build Android release candidate.
6. Archive artifact and build metadata.
7. Publish only through an explicitly approved release workflow.

Store credentials must be injected through protected secrets.

## 9. Release checklist

- [ ] Release branch/tag created from approved commit.
- [ ] Version name/code incremented.
- [ ] Release notes prepared.
- [ ] Unity/device regression complete.
- [ ] Backend production smoke test complete.
- [ ] Analytics smoke test complete.
- [ ] Ads/IAP test complete.
- [ ] Signed `.aab` generated.
- [ ] Store listing reviewed.
- [ ] Privacy/data-safety declarations reviewed.
- [ ] Internal test accepted.
- [ ] Closed test accepted.
- [ ] Production staged rollout approved.
- [ ] Post-release monitoring active.

## 10. Rollback / hotfix

If a production issue is detected:

- Stop further rollout.
- Identify affected version/build.
- Disable problematic remote configuration where safe.
- Prepare a minimal hotfix branch from the production tag.
- Regression-test the affected area and critical startup/game flows.
- Release a new version through the normal controlled track process.
- Document root cause and corrective action.

## 11. Post-launch monitoring

Track crashes, ANRs, startup failures, session completion, retention, game-specific errors, backend failures, economy anomalies, leaderboard submission failures, and IAP/ads failures. Use aggregated telemetry and privacy-preserving practices.
