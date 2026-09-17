# V1.0 Release Readiness

## Project
- [ ] Set final Unity version and verify the project opens without compiler errors.
- [ ] Confirm Android package identifier and application version `1.0.0`.
- [ ] Confirm ARM64 Android architecture.
- [ ] Confirm portrait/landscape orientation matches the final UI.

## Gameplay QA
- [ ] Play each of the 10 mini-games from the hub.
- [ ] Verify start, timer, score, game-over, retry and navigation flows.
- [ ] Verify pause/resume and time-scale recovery.
- [ ] Verify best scores persist after restart.
- [ ] Verify coins and games-played persistence.
- [ ] Verify sound and haptics settings persist and are respected.

## Performance
- [ ] Test on at least one lower-end Android device.
- [ ] Test on at least one mid/high-range Android device.
- [ ] Check frame pacing and input responsiveness.
- [ ] Check memory usage during repeated game restarts.
- [ ] Confirm there are no persistent duplicate runtime systems.

## Release hygiene
- [ ] Remove debug-only content and temporary assets.
- [ ] Keep generated Unity folders out of Git.
- [ ] Keep files >=100 MB in Git LFS when source assets require them.
- [ ] Create a Development Build for final device QA before Release Build.
- [ ] Create the final Release APK/AAB only after device QA passes.

## Important
GitHub integration cannot run the Unity compiler, build pipeline, profiler, or Android device tests. This checklist is therefore a release gate, not a claim that those tests have already passed.
