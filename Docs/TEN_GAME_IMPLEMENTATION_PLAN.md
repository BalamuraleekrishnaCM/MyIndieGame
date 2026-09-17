# Ten Game Implementation Checklist

The repository now contains one production runtime core for each catalog game. Each core is responsible for deterministic gameplay state and delegates lifecycle/score handling to the shared production base.

## Runtime cores

- Tap Rush — existing migrated core
- Color Match — color selection and round scoring
- Stack It — timing/alignment and tower progression
- Dodge Line — lane movement, dodge detection and lives
- Coin Catch — coin capture scoring
- Memory Flip — pair matching
- One Tap Jump — jump, obstacle and lives state
- Ball Sort — tube transfer and solved-tube detection
- Parking Puzzle — grid movement, exit state and completion
- Merge 2048 — 4x4 tile movement, merging and target detection

## Per-game Unity integration still required

1. Add/verify the corresponding scene and prefab.
2. Bind UI/touch controls to the public gameplay methods.
3. Bind score and completion events to the shared HUD/results flow.
4. Add visual feedback, audio and haptics through shared services.
5. Verify safe-area layout and touch target sizes.
6. Verify restart, pause/resume, scene transition and persistence behavior.
7. Run Play Mode and Android device regression.

These are runtime-environment gates and are not represented as completed merely by the presence of the C# cores.
