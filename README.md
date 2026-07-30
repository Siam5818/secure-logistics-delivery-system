\# Secure Logistics \& Delivery System



Microservices-based logistics and delivery platform built with ASP.NET Core,

following Onion Architecture and Domain-Driven Design principles.



\## Status

🚧 Project under active development.



\## Documentation

See `/docs` for architecture decisions (ADR), setup guide, and diagrams.


## Code Standards

This project enforces consistent code style and quality via:
- `.editorconfig` — code style rules (indentation, brace style, `var` usage)
- `src/Directory.Build.props` — centralized build settings applied to all
  projects (nullable reference types, warnings treated as errors, .NET
  analyzers at `latest-recommended` level)

All warnings are treated as build errors. A failing build due to a code
analysis warning is expected behavior, not a bug — fix the warning rather
than suppressing it unless there is a documented justification.

## Local Infrastructure

This project uses Docker Compose to run required infrastructure locally.

### Prerequisites
Copy the example environment file and adjust values as needed:

\`\`\`powershell
Copy-Item .env.example .env
\`\`\`

### Start infrastructure

\`\`\`powershell
docker compose up -d
\`\`\`

### Verify Order Service database is running

\`\`\`powershell
docker compose ps
\`\`\`

### Stop infrastructure

\`\`\`powershell
docker compose down
\`\`\`

To also remove persisted data volumes:

\`\`\`powershell
docker compose down -v
\`\`\`
