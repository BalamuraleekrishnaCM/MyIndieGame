# Analytics and Live-Ops Specification

## Core Events
game_started, game_completed and reward_claimed.

Recommended metadata: stable game ID, session ID, score, duration, app version, platform and configuration version where privacy policy permits.

## Funnels
Home → game open → game start → completion → replay. Track abandonment and error states.

## Remote Configuration
Safe tunables include timers, spawn rates, difficulty, offers and feature flags. Every remote value needs a local default and validation range.

## Experiments
Use stable assignment. Never let experiments change core IDs or service contracts.

## Operations
Monitor crashes, ANRs, completion rates, service failures, purchase failures, ad failures and abnormal score/reward patterns.

## Privacy
Analytics requires consent and must comply with the published privacy policy and applicable platform rules.
