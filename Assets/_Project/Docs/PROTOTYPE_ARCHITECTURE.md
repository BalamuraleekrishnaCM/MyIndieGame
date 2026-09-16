# Prototype Architecture

```text
MiniGameCollection
 ├── Hub
 ├── Session timer
 ├── Score
 ├── Result flow
 └── Mini-game handlers
      ├── Tap Rush
      ├── Color Match
      ├── Stack It
      ├── Dodge Line
      ├── Coin Catch
      ├── Memory Flip
      ├── One Tap Jump
      ├── Ball Sort
      ├── Parking Puzzle
      └── Merge 2048
```

The prototype intentionally keeps the first pass in one controller. This makes the collection easy to test and avoids premature asset/package complexity. Once the mechanics are validated, each game can be extracted into a dedicated `MiniGameBase` implementation without changing the hub contract.
