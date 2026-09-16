# Mini Games

The collection currently contains 10 games registered in `MiniGameCatalog`.

Each game should implement only its own rules and presentation, while using the Core systems for session lifecycle, score persistence, input and feedback.

## V0.3 feel hooks

Use `GameFeel.Punch` after successful interactions, `GameFeel.ScreenShake` on failure/impact, and `HapticFeedback.Light()` for mobile feedback. Keep effects short so the games remain responsive and casual.
