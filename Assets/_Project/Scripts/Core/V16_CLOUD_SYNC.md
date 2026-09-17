# V1.6 Cloud Sync

## Implemented
- Cloud player snapshot model from the existing local identity, session and player statistics services.
- Cloud sync coordinator that creates a safe local snapshot and tracks pending synchronization.
- Existing provider-neutral `CloudSyncService` remains the transport boundary.
- Offline-first behavior: failed or unavailable pushes remain marked as pending.

## Sync contract
```text
Local Player State
      |
      v
CloudSyncCoordinator
      |
      v
CloudSyncService
      |
      v
ICloudSyncProvider
      |
      v
Cloud Backend
```

## Conflict policy
The client must not blindly overwrite newer server state. A production adapter must compare a server revision/timestamp and apply an explicit merge policy. Currency, purchases, competitive scores and challenge completion must be server-authoritative.

## Production gates
- Connect the selected backend adapter.
- Add authenticated requests using short-lived/refreshable sessions.
- Add server-side ownership checks and RLS.
- Implement revision-based conflict resolution.
- Sync after authentication and on controlled lifecycle points.
- Retry transient failures with bounded backoff.
- Never expose service-role/secret credentials in the Unity client.
- Verify cloud sync on Android with offline/online transitions.

## Validation
This milestone provides the client-side synchronization foundation only. GitHub integration cannot compile Unity or execute backend/device integration tests. Live backend synchronization remains a production integration gate.
