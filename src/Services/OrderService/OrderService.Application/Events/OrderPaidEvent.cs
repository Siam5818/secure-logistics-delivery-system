namespace OrderService.Application.Events;

public sealed record OrderPaidEvent(Guid OrderId, decimal Amount, string Currency, DateTime PaidAtUtc);
