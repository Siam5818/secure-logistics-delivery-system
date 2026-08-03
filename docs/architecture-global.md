# Global Architecture — Secure Logistics & Delivery System

## System overview

                      ┌─────────────────-┐
                      │   React Frontend │
                      │  (localhost:5173)│
                      └────────┬─────────┘
                               │ HTTP
                               ▼
                      ┌─────────────────-┐
                      │   API Gateway    │
                      │      (YARP)      │
                      │ (localhost:5007) │
                      └────────┬─────────┘
                               │ HTTP (routed)
                ┌──────────────┴──────────────┐
                ▼                             ▼
      ┌─────────────────--─-┐          ┌──────────────────--┐
      │     Order Service   │          │  Delivery Service  │
      │   (localhost:5114)  │          │  (localhost:5080)  │
      │  Onion Architecture │          │ Onion Architecture │
      └─────────┬─────────--┘          └──────────┬─────────┘
                │                                 │
                ▼                                 ▼
      ┌──────────────────┐          ┌──────────────────-─┐
      │  orderservice-db │          │ deliveryservice-db │
      │   (PostgreSQL)   │          │    (PostgreSQL)    │
      └──────────────────┘          └────────────────────┘

                │  publishes OrderPaidEvent
                ▼
      ┌─────────────────-──┐
      │      RabbitMQ      │
      │  (localhost:15672) │
      └─────────┬──────────┘
                │  consumed by
                ▼
      Delivery Service (OrderPaidEventConsumer)

## Component responsibilities

| Component | Role |
|---|---|
| React Frontend | Single UI module (create/pay orders), consumes only the Gateway |
| API Gateway (YARP) | Single entry point, routes to Order/Delivery Service |
| Order Service | Order lifecycle: creation, lines, payment. Publishes OrderPaidEvent |
| Delivery Service | Listens for OrderPaidEvent, creates deliveries automatically |
| RabbitMQ | Asynchronous event bus between services (no direct HTTP coupling) |
| PostgreSQL (x2) | One database per service — no shared schema |
| Keycloak | Deployed but unused (see ADR-002) — JWT generated internally instead |

## Cross-cutting concerns

- **Onion Architecture** in both services: Domain → Application → Infrastructure → API
- **Authentication**: JWT Bearer (Order Service only), with RBAC (Admin/Customer roles)
- **Health Checks**: `/health` on both services (PostgreSQL connectivity)
- **CI**: GitHub Actions (restore/build/test on PRs)
