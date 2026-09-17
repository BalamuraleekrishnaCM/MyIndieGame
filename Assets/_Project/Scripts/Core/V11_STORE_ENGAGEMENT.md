# V1.1 Store & Engagement Foundation

V1.1 adds provider-neutral foundations for monetization and retention without introducing a third-party SDK.

## Store
- `StoreCatalog` defines stable product IDs for coin packs and a remove-ads entitlement.
- `StoreCatalog.GrantCoins` centralizes coin fulfillment after a verified purchase callback.
- Product prices are intentionally not stored in code; configure prices in the relevant store console/provider.

## Rewarded ads
- `RewardedAdService` provides a single integration boundary for a rewarded-ad provider.
- The prototype returns unavailable until a real ad SDK is integrated.
- Rewards must only be granted from a verified completed-ad callback; `ShowRewardedAd` does not grant coins.
- `MarkAdsRemoved` stores the remove-ads entitlement locally for prototype use. Production purchases should be verified by the selected billing provider and restored on reinstall/device change.

## Daily rewards
- `DailyRewardService` provides one claim per UTC calendar day.
- Streak rewards progress from 10 coins up to 40 coins over a seven-day cycle.
- The service persists the last claim date and streak using `PlayerPrefs`.

## Production integration gates
- Select and integrate the official Google Play Billing solution for Android purchases.
- Select an ad provider and integrate rewarded ads with server/provider-side verification where supported.
- Add purchase restore/entitlement recovery.
- Add privacy consent flow and store-required disclosures before serving personalized ads where applicable.
- Do not treat `PlayerPrefs` as authoritative proof of a real-money purchase.
- Test purchases and ads with the provider's test environment before release.

## Scope
No third-party SDK, API key, network service, or real-money transaction is included in V1.1. This keeps the core project buildable without external monetization dependencies while defining stable integration points for the release build.
