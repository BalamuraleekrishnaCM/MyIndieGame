# V1.4 Cloud Backend Foundation

## Included
- Provider-neutral cloud synchronization interface.
- Cloud player data model for identity, economy, progression and statistics.
- PostgreSQL schema draft for profiles, scores and challenges.

## Recommended production architecture
- Authentication issues a user identity/token.
- Client uses only a publishable/public client key.
- Row-level security limits user-owned data access.
- Server/Edge Functions validate score submissions and economic rewards.
- Leaderboard reads can be public or authenticated depending on the product design.
- Service-role credentials must never ship inside the Unity client.

## Supabase option
Supabase provides Auth, Postgres, RLS and Edge Functions suitable for this architecture. Its current C# client is community-maintained, and the current C# package line targets netstandard2.1; verify Unity compatibility before adding the dependency.

## Production gates
- Select and create the production backend project.
- Configure authentication and deep-link/redirect settings.
- Apply migrations and RLS policies.
- Implement the C# provider adapter.
- Add conflict resolution for offline/online saves.
- Server-validate competitive scores and economy changes.
- Test account deletion, token expiry, offline mode, reconnect, duplicate requests and migration rollback.
