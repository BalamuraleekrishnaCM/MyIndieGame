# V0.9 — Audio, VFX & Game Feel

## Included
- Reusable `VFXFeedback` for hit punches and lightweight screen flashes.
- Central `FeedbackService` for success, fail, and click feedback.
- Audio feedback now respects the persistent sound preference.
- Haptics now respect the persistent haptics preference.
- Core systems are bootstrapped on a persistent `AppSystems` object.

## Asset policy
V0.9 adds systems only; no external audio or visual assets are required. Audio clips can be assigned later through the existing `AudioFeedback` serialized fields.

## Manual validation
1. Open the project in Unity and enter Play Mode.
2. Confirm the `AppSystems` object persists between scene loads and has no duplicate core systems.
3. Assign optional success/fail/click clips and verify sound toggling.
4. Verify haptic toggling on a physical Android/iOS device.
5. Exercise feedback calls during gameplay and confirm there are no null-reference errors.
6. Test with timeScale paused to confirm feedback coroutines use unscaled time.
