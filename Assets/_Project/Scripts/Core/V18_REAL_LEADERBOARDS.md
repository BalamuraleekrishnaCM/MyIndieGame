# V1.8 Real Leaderboards

## Implemented
- Provider-neutral leaderboard contracts.
- Top leaderboard and around-player queries.
- Local personal-best fallback for offline play.
- In-memory remote result cache for the current session.
- Remote score submission with client-side idempotency keys.
- HTTP provider using UnityWebRequest; no third-party runtime SDK required.
- Bearer-token injection through a runtime callback; no secrets are stored in the project.
- Backend PostgreSQL schema draft with unique player/game/season scores and durable idempotency keys.

## API contract
- `GET /leaderboards/{gameId}?seasonId={seasonId}&limit={limit}`
- `GET /leaderboards/{gameId}/around-me?seasonId={seasonId}&limit={limit}`
- `POST /leaderboards/submit`

Submit body:
```json
{"gameId":"tap-rush","seasonId":"global","score":1200,"idempotencyKey":"..."}
```

Response body:
```json
{"gameId":"tap-rush","seasonId":"global","limit":50,"offset":0,"totalCount":100,"entries":[{"PlayerId":"...","DisplayName":"Player","Score":1200,"Rank":1}]}
```

## Authority and security
The server is authoritative for rank and score acceptance. The client must never choose `player_id` from request data, write directly to the leaderboard table, or treat a local score as an online ranking.

The backend must authenticate the caller, validate the game/season, enforce score bounds, atomically handle the idempotency key, and only replace a player's stored score when the submitted score is higher.

## Integration
Configure the provider at runtime:
```csharp
LeaderboardService.Configure(
    new HttpLeaderboardProvider(
        "https://your-api.example.com",
        () => accessToken));
```

Until a production endpoint is configured, the game remains safe to run offline and exposes local personal-best data only.

## Release gates
- Select and deploy the production backend.
- Add authenticated server endpoints/functions.
- Enable RLS/authorization according to the chosen provider.
- Run duplicate submission, retry, offline/reconnect, ranking and abuse tests.
- Compile Unity project and test on Android devices.
