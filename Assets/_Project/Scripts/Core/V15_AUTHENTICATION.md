# V1.5 Authentication & Player Identity

## Implemented
- Provider-neutral `PlayerIdentityService` for user ID and display name.
- Provider-neutral `AuthenticationService` boundary for sign-in and sign-out.
- Local identity persistence for continuity between launches.
- Safe behavior when no authentication provider is configured.
- No service-role credentials, secrets, or third-party SDKs in the Unity client.

## Production integration
The provider adapter should be implemented only after the selected authentication backend is finalized and its Unity package compatibility is verified.

Recommended production flow:

```text
Unity Client
    |
    v
AuthenticationService
    |
    v
Provider Adapter
    |
    v
Authentication Backend
    |
    v
Authenticated User ID
```

## Security requirements
- Never trust a client-supplied user ID for authorization.
- Use the backend-issued authenticated subject as the canonical identity.
- Keep privileged/service-role credentials off-device.
- Validate access tokens server-side.
- Apply row-level ownership rules to player data.
- Support sign-out and session expiry.
- Avoid storing passwords or long-lived provider secrets in PlayerPrefs.

## Remaining gates
- Select production identity provider.
- Implement provider adapter.
- Add login/logout UI.
- Verify token refresh and expiry.
- Connect identity to cloud player profile.
- Test account recovery where applicable.
- Test account deletion/privacy requirements.
- Verify RLS ownership policies against authenticated user IDs.
- Test offline launch and expired-session behavior on Android.

## Validation
GitHub integration cannot compile Unity or execute platform authentication flows. Unity compilation, provider SDK compatibility, authentication, session handling, RLS and device testing remain release gates.
