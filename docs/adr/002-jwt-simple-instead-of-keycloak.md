# ADR-002: Use simple JWT instead of Keycloak for Order Service authentication

## Status
Accepted

## Context
SLDS-10 planned Keycloak as the identity provider for Order Service,
with an explicit 1-day time-box agreed in the product vision (SLDS-2)
given the project's tight timeline.

Keycloak was successfully deployed via Docker Compose (realm, client,
and test user configured), but the `password` grant type consistently
returned `invalid_grant: Account is not fully set up`, even after
verifying user credentials, required actions, email verification, and
client "Direct access grants" settings. Root cause was not identified
within the time-box.

## Decision
Fall back to a simple JWT generated internally by OrderService.Api,
as agreed in the time-box rule. A minimal `/api/Auth/token` endpoint
issues signed tokens for testing purposes; standard ASP.NET Core JWT
Bearer middleware validates them on protected endpoints.

## Consequences
- No real user authentication (no password verification) — acceptable
  for this MVP/exam scope, documented as known technical debt
- The Keycloak container remains in docker-compose.yml for future
  investigation, but is not used by the application
- If time allows later, revisit the Keycloak `password` grant issue,
  or migrate to the Authorization Code flow instead
