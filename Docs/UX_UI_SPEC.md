# UX and UI Specification

## Global Screens
### Home
Game catalog, discovery state, best score, settings and optional progression/reward surfaces.

### Game
Clear score/status, play area, pause/settings affordance where applicable and accessible feedback.

### Results
Final score, best score, improvement, replay, next game and home.

## Interaction Rules
- Touch targets must be comfortably tappable.
- Input feedback must be immediate.
- Drag controls require visible affordance.
- Back should pause or confirm exit rather than silently discard a run.
- Critical state must not rely on color alone.

## Accessibility
Reduced motion suppresses nonessential motion. Large text must not clip or destroy hierarchy. Audio and haptics should have independent controls where exposed. Contrast must remain readable.

## Safe Areas
All mobile UI must respect Screen.safeArea and be checked against notches, rounded corners and navigation areas.

## UX Acceptance
No clipped essential controls, ambiguous game state, inaccessible required action or result screen without clear outcome.
