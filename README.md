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

## Domain Model

Order Service's domain model (`OrderService.Domain`) implements Order as
an Aggregate Root with OrderLine entities and Money/Address value objects.
Business invariants are enforced through private setters and explicit
methods (e.g. `Order.MarkAsShipped()`), never through public property
mutation. Run `dotnet test src/Services/OrderService/OrderService.Domain.Tests`
to validate all invariants.

## Persistence

Order Service uses EF Core with PostgreSQL (Npgsql provider). Entity
identifiers are generated in the Domain layer (not by the database),
requiring explicit `ValueGeneratedNever()` configuration. Apply migrations:

\`\`\`powershell
dotnet ef database update --project src/Services/OrderService/OrderService.Infrastructure --startup-project src/Services/OrderService/OrderService.Api
\`\`\`

## Authentication

Order Service uses simple JWT authentication (see ADR-002 for context —
Keycloak integration was attempted but reverted after exceeding its
time-box). Obtain a test token via `POST /api/Auth/token` with a
`username`, then use it as a Bearer token on protected endpoints.

## Event-driven messaging

Order Service publishes `OrderPaidEvent` via MassTransit/RabbitMQ when
an order is marked as paid. RabbitMQ Management UI: http://localhost:15672
(guest/guest). MassTransit is pinned to 8.3.4 (pre-license v9 change).

## API Gateway

All client traffic goes through the API Gateway (YARP) at
http://localhost:5007 (dev) — routes /api/orders/** and /api/auth/**
to Order Service. Dockerization of the Gateway is defined in
docker-compose.yml but not yet fully tested in-container (technical debt).

## Project Structure

- `src/` — backend (.NET): Order Service (Onion Architecture) + API Gateway
- `frontend/` — React (Vite + TypeScript) client consuming the API Gateway

## Frontend

```powershell
cd frontend
npm install
npm run dev
```

Runs on http://localhost:5173. Requires the API Gateway (port 5007) and
Order Service (port 5114) running in parallel. The frontend is a single
merged module (no separate Client/Driver apps), per the MVP scope
defined in the product vision.

## Delivery Service

Second microservice, replicating the Onion Architecture pattern from
Order Service with a reduced scope (no dedicated auth, no Gateway route
yet). Listens to `OrderPaidEvent` (via Shared.Contracts) and automatically
creates a Delivery record. Runs on port 5080.

Check a delivery: `GET http://localhost:5080/api/Deliveries/{orderId}`
