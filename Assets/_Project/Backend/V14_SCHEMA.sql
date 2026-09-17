-- V1.4 cloud backend foundation
-- Provider-neutral PostgreSQL schema. Apply through the selected backend's migration system.

create table if not exists public.player_profiles (
    user_id uuid primary key,
    display_name text not null default 'Player',
    coins integer not null default 0 check (coins >= 0),
    games_played integer not null default 0 check (games_played >= 0),
    total_score bigint not null default 0 check (total_score >= 0),
    wins integer not null default 0 check (wins >= 0),
    updated_at timestamptz not null default now()
);

create table if not exists public.game_scores (
    id bigint generated always as identity primary key,
    user_id uuid not null references public.player_profiles(user_id) on delete cascade,
    game_id text not null,
    score integer not null check (score >= 0),
    created_at timestamptz not null default now()
);

create index if not exists game_scores_game_score_idx
    on public.game_scores (game_id, score desc);

create table if not exists public.player_challenges (
    user_id uuid not null references public.player_profiles(user_id) on delete cascade,
    challenge_id text not null,
    progress integer not null default 0 check (progress >= 0),
    completed boolean not null default false,
    updated_at timestamptz not null default now(),
    primary key (user_id, challenge_id)
);

-- Security requirements for a Supabase deployment:
-- 1. Enable RLS on every exposed table.
-- 2. Restrict profile reads/updates to auth.uid() = user_id.
-- 3. Do not allow clients to directly award coins or wins.
-- 4. Validate competitive scores server-side before leaderboard insertion.
-- 5. Keep service-role credentials server-side only.
