# Production game core boundary

The runtime layer has one C# controller for every catalog game. Controllers are presentation-agnostic and expose deterministic interaction/state APIs. Unity scenes and prefabs bind those APIs to touch controls and visuals.
