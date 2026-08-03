# Demo Script — Secure Logistics & Delivery System

## 1. Fresh environment startup

```powershell
docker compose down -v
docker compose up -d orderservice-db deliveryservice-db keycloak rabbitmq
```

Apply migrations:
```powershell
cd src
dotnet ef database update --project Services/OrderService/OrderService.Infrastructure --startup-project Services/OrderService/OrderService.Api
dotnet ef database update --project Services/DeliveryService/DeliveryService.Infrastructure --startup-project Services/DeliveryService/DeliveryService.Api
```

## 2. Launch services (4 terminals)

```powershell
dotnet run --project Services/OrderService/OrderService.Api
dotnet run --project Services/DeliveryService/DeliveryService.Api
dotnet run --project Gateway/ApiGateway
```
```powershell
cd frontend && npm run dev
```

## 3. Demo flow (via UI at localhost:5173)

1. Fill in the "Create Order" form → click **Create Order**
2. Add a line (product/quantity/price) → click **Add Line**
3. Click **Pay Order** → status turns "Paid"
4. Open a terminal and check Delivery Service was notified:

GET http://localhost:5080/api/Deliveries/{orderId}

→ returns the automatically created Delivery (status: Pending)

## 4. Talking points for the jury

- **Onion Architecture**: show `Order.cs` (Domain) — no external dependency, invariants enforced via private setters
- **Event-driven**: show RabbitMQ Management UI (localhost:15672) — the queue and message flow
- **Database-per-service**: show two separate `docker compose ps` entries, two separate connection strings
- **RBAC**: request an Admin token, call `DELETE /api/Orders/{id}` — show 403 vs 204 depending on role
- **Technical debt, assumed and documented**: ADR-002 (Keycloak → JWT fallback), demo-only delete endpoint
