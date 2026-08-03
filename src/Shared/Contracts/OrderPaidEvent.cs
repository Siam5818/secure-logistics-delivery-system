namespace Contracts;

public sealed record OrderPaidEvent(Guid OrderId, decimal Amount, string Currency, DateTime PaidAtUtc);
