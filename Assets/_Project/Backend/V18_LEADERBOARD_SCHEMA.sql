-- V1.8 Real Leaderboards
-- Provider-neutral PostgreSQL/Supabase-compatible schema draft.
-- The server/API must authenticate the caller and derive player_id from the session.

create table if not exists public.leaderboard_scores (
    id uuid primary key default gen_random_uuid(),
    game_id text not null,
    season_id text not null default 'global',
    player_id uuid not null,
    display_name text not null default 'Player',
    score integer not null check (score >= 0),
    submitted_at timestamptz not null default now(),
    updated_at timestamptz not null default now(),
    unique (game_id, season_id, player_id)
);

create index if not exists leaderboard_scores_rank_idx
    on public.leaderboard_scores (game_id, season_id, score desc, updated_at asc);

create table if not exists public.leaderboard_submission_keys (
    idempotency_key text primary key,
    game_id text not null,
    season_id text not null default 'global',
    player_id uuid not null,
    score integer not null check (score >= 0),
    created_at timestamptz not null default now()
);

create index if not exists leaderboard_submission_player_idx
    on public.leaderboard_submission_keys (player_id, created_at desc);

-- RLS should be enabled in the selected backend.
-- Clients must not receive direct write access to leaderboard_scores.
-- Expose only authenticated server/API functions such as:
--   GET  /leaderboards/{gameId}?seasonId=...&limit=...
--   GET  /leaderboards/{gameId}/around-me?seasonId=...&limit=...
--   POST /leaderboards/submit
-- The submit operation must atomically:
-- 1. authenticate player_id from the session;
-- 2. reject replayed idempotency_key;
-- 3. validate game_id/season_id and score bounds;
-- 4. upsert only when the new score is higher;
-- 5. record the idempotency key;
-- 6. return the authoritative result.

-- Example ranking query:
-- select player_id, display_name, score,
--        dense_rank() over (order by score desc, updated_at asc) as rank
-- from public.leaderboard_scores
-- where game_id = $1 and season_id = $2
-- order by score desc, updated_at asc
-- limit $3;
