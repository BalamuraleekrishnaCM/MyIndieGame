# V1.2 Analytics, Achievements & Missions

## Included
- Provider-neutral analytics event boundary.
- Persistent local achievements with four starter milestones.
- Daily missions requiring 3 games and 100 cumulative score, with a 20-coin claim reward.
- Local persistence through PlayerPrefs so the systems work without third-party services.

## Production integration gates
- Replace the analytics event listener with the selected analytics provider.
- Do not send personally identifiable information in event parameters.
- Validate consent/privacy requirements before enabling production analytics.
- Add a proper achievements UI and mission UI in Unity.
- Test day rollover, app restart, duplicate reward claims, and corrupted/missing PlayerPrefs values.
- Add server-side validation if rewards become economically meaningful.

## Validation note
GitHub integration cannot compile Unity or run device tests. Unity compilation and mobile QA remain local gates.
