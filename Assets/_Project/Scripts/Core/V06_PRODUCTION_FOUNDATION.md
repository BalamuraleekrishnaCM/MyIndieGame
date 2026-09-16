# V0.6 Production Foundation

This milestone prepares the prototype for a cleaner mobile production workflow without requiring external assets.

## Runtime systems

- `SceneBootstrap` creates persistent core systems before the first scene.
- `ProductionBootstrap` ensures mobile performance settings are installed after a scene loads.
- `MobilePerformance` disables VSync and sets a configurable mobile target frame rate (60 FPS by default).
- `AppSettings` persists sound and haptic preferences.

## Scene strategy

The current prototype can continue using its runtime-generated UI scene. V0.6 establishes reusable runtime systems first; individual mini-games can be separated into scenes in a later milestone without rewriting persistence.

## Android checklist

1. Set the desired Android package identifier and application version in Player Settings.
2. Use ARM64 for release builds where the selected Unity/Android configuration supports it.
3. Test at 60 FPS on the target device and reduce the target when thermal/battery constraints require it.
4. Verify touch input on a physical Android device.
5. Keep generated builds, Library, Temp, Logs, and other generated Unity data out of Git.
6. Use Git LFS only for genuinely large binary source assets according to the repository policy.

## Manual validation

Open the project in the intended Unity version, enter Play Mode, launch the existing game hub, and verify that the runtime bootstrap does not create duplicate systems. Then make an Android Development Build and verify frame pacing, touch input, pause/resume, persistence, and orientation.
