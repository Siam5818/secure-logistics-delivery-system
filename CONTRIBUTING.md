\# Contributing Guide



\## Branching Strategy



This project follows a simplified GitFlow model:



\- `main` — stable, protected branch. Never committed to directly.

\- `develop` — integration branch for ongoing work.

\- `feature/SLDS-x-short-description` — one branch per Jira ticket

&#x20; (Story or Task), created from `develop`.



\### Branch naming convention

Example: `feature/SLDS-5-order-domain-model`

## Commit Convention

This project uses [Conventional Commits](https://www.conventionalcommits.org/):

- `feat:` — new feature
- `fix:` — bug fix
- `chore:` — maintenance, tooling, configuration
- `docs:` — documentation only
- `test:` — adding or fixing tests
- `refactor:` — code change that neither fixes a bug nor adds a feature

