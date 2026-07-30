# Architecture Overview

## Onion Architecture

Each microservice in this project follows the Onion Architecture pattern.
Dependencies always point inward, toward the Domain layer, never the
other way around.

```
API → Infrastructure → Application → Domain
```

### Layer responsibilities

| Layer | Responsibility | Depends on |
|---|---|---|
| **Domain** | Business entities and core rules. Pure C# classes, no external dependencies (no ASP.NET, no EF Core, no database driver). | Nothing |
| **Application** | Use cases / business workflows. Defines interfaces (e.g. `IOrderRepository`) that Infrastructure will implement. | Domain |
| **Infrastructure** | Concrete implementations of Application interfaces: database access (EF Core/PostgreSQL), external services, messaging. | Application, Domain |
| **API** | Entry point. REST controllers, dependency injection configuration, request/response DTOs. | Application, Infrastructure, Domain |

### Why this matters

Swapping a technical detail (e.g. PostgreSQL → MongoDB, or Entity Framework →
Dapper) only requires changes in the Infrastructure layer. The Domain layer,
which holds the actual business rules, remains untouched and fully testable
without any database.

## Current services

### Order Service

```
src/Services/OrderService/
├── OrderService.Domain
├── OrderService.Application
├── OrderService.Infrastructure
└── OrderService.Api
```

Reference project for the pattern. Delivery Service will replicate this
exact structure once validated.
