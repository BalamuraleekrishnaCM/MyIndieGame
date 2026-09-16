# MyIndieGame 🎮

A simple and addictive hyper-casual mobile game collection made for fun, experimentation, and rapid gameplay iteration.

## 🎯 Current Prototype

The prototype contains a single game hub with 10 quick casual mini-games:

1. Tap Rush — tap a moving target.
2. Color Match — select the requested color.
3. Stack It — stop the moving block at the right moment.
4. Dodge Line — switch lanes and avoid the obstacle.
5. Coin Catch — collect coins and avoid hazards.
6. Memory Flip — find matching pairs.
7. One Tap Jump — time jumps over obstacles.
8. Ball Sort — move balls through the sorting challenge.
9. Parking Puzzle — clear the route and park the car.
10. Merge 2048 — merge equal-number tiles.

## 🛠️ Technology

- Unity
- C#
- Mobile-first UI
- No external package dependency for the prototype

## 📁 Unity Structure

```text
Assets/
└── _Project/
    └── Scripts/
        └── MiniGames/
            └── MiniGameCollection.cs
```

## ▶️ Prototype Setup

1. Create/open a Unity project.
2. Add `Assets/_Project/Scripts/MiniGames/MiniGameCollection.cs`.
3. Create an empty GameObject in a scene.
4. Add a Canvas component to it.
5. Add `MiniGameCollection` to the same GameObject.
6. Press Play.

The script creates the prototype UI at runtime, so no imported art package is required.

## 🚧 Status

**V0.2 — Playable mechanics prototype.**

The ten games now have individual interactions, scoring, timers, retry/home flow, and basic win/lose states. Visual assets, audio, haptics, persistence, and deeper game-specific mechanics are planned for subsequent iterations.

## 🌿 Git Flow

```text
main
  ↑
develop
  ↑
feature/gameplay-polish
```

## 📌 Development Principles

1. Prototype first.
2. Keep each game understandable within seconds.
3. Reuse shared systems where practical.
4. Optimize for mobile performance.
5. Polish only after the core mechanic is fun.
