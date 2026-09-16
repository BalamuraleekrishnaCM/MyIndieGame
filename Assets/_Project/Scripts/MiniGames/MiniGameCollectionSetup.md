# Prototype Setup

1. Create a normal Unity 3D or 2D project.
2. Copy `MiniGameCollection.cs` into `Assets/_Project/Scripts/MiniGames/`.
3. Create an empty GameObject named `MiniGameApp`.
4. Add a Canvas component to it and set Render Mode to `Screen Space - Overlay`.
5. Add `MiniGameCollection` to the same object.
6. Add an `EventSystem` to the scene if Unity does not create one automatically.
7. Press Play. The hub presents all 10 games.

The prototype intentionally uses Unity's built-in UI and emoji/text placeholders so the gameplay systems can be replaced with final art later.

## Games

Tap Rush, Color Match, Stack It, Dodge Line, Coin Catch, Memory Flip, One Tap Jump, Ball Sort, Parking Puzzle, and Merge 2048.

## Architecture

The first prototype keeps the hub and mini-games in one runtime controller for fast iteration. Later versions should split each game into its own scene/prefab and share common services for navigation, score, audio, save data, ads, and analytics.
