# V1.6 Real Cloud Sync Foundation

## Implemented
- Versioned `CloudPlayerData` model for identity, economy, progression and statistics.
- Provider-neutral cloud sync lifecycle with pull/push states.
- Offline-safe behavior when no authenticated provider is available.
- Local snapshot generation from the current player identity, economy and statistics.
- Last cloud snapshot retained in memory for a future conflict-resolution adapter.

## Sync contract
```text
Authenticated Player
        |
        v
   CloudSaveSnapshot
        |
        v
    CloudSyncService
       /     \
    Pull       Push
     |           |
     v           v
 Provider Adapter / Backend
```

## Conflict strategy boundary
The client does not blindly overwrite cloud state. The provider/backend must decide the authoritative version using server-side versioning or timestamps. Economy, competitive scores and rewards must be validated server-side before production sync.

## Production gates
- [ ] Select production backend project/provider.
- [ ] Implement authenticated provider adapter.
- [ ] Create/apply database migrations.
- [ ] Enable RLS on every exposed player-data table.
- [ ] Bind ownership to the backend-authenticated subject.
- [ ] Implement server-side conflict resolution.
- [ ] Sync best scores, achievements, missions and progression.
- [ ] Handle offline queue/retry and duplicate requests safely.
- [ ] Test token expiry, reconnect, account deletion and migration rollback.
- [ ] Verify Android build/package compatibility.

## Security
- Unity must use only a publishable/public client credential where applicable.
- Service-role/secret credentials must never ship in the app.
- Client-submitted user IDs must never authorize access to another player's data.
- Server validation is required for economy and competitive values.

## Supabase note
Supabase is documented as a possible backend, but this milestone intentionally does not connect the game to the existing Supabase project. A production project must be explicitly selected before migrations or live data are created.

## Validation boundary
GitHub integration cannot compile Unity or run online/device integration tests. Unity compilation, provider SDK compatibility, RLS, sync conflict tests and Android testing remain release gates.
