using MassTransit;
using Contracts;

namespace OrderService.Api.Consumers;

public sealed partial class OrderPaidConsumer : IConsumer<OrderPaidEvent>
{
    private readonly ILogger<OrderPaidConsumer> _logger;

    public OrderPaidConsumer(ILogger<OrderPaidConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<OrderPaidEvent> context)
    {
        var evt = context.Message;
        LogOrderPaid(evt.OrderId, evt.Amount, evt.Currency, evt.PaidAtUtc);
        return Task.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Information,
        Message = "OrderPaidEvent received: OrderId={OrderId}, Amount={Amount} {Currency}, PaidAt={PaidAt}")]
    private partial void LogOrderPaid(Guid orderId, decimal amount, string currency, DateTime paidAt);
}
