# V1.5 Authentication Checklist

## Implemented
- [x] Provider-neutral authentication interface.
- [x] Player identity model and local display identity persistence.
- [x] Explicit authentication lifecycle states.
- [x] Session model with non-persistent expiry metadata.
- [x] Sign-in/sign-out controller.
- [x] Offline launch remains signed out until a provider validates a session.
- [x] Development authentication provider for local integration testing.

## Production gates
- [ ] Select production identity provider.
- [ ] Implement and package the production provider adapter.
- [ ] Validate backend-issued user ID and access tokens.
- [ ] Implement token refresh and expiry handling.
- [ ] Add login/logout UI and error states.
- [ ] Connect authenticated identity to cloud player data.
- [ ] Verify account recovery and deletion/privacy flows where applicable.
- [ ] Verify RLS ownership against the authenticated subject.
- [ ] Test offline launch, expired sessions and sign-out on Android.

## Security rules
- Never use the development provider in a release build.
- Never store access/refresh tokens or provider secrets in PlayerPrefs.
- Never trust a client-supplied user ID for authorization.
- Keep privileged/service-role credentials off-device.

## Validation boundary
GitHub integration cannot compile Unity or execute platform authentication/device tests. Unity compilation, provider SDK compatibility, session refresh, backend authorization and Android testing remain release gates.
