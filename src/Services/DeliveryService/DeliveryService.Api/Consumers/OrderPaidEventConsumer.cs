using MassTransit;
using Contracts;
using DeliveryService.Application.UseCases;

namespace DeliveryService.Api.Consumers;

public sealed partial class OrderPaidEventConsumer : IConsumer<OrderPaidEvent>
{
    private readonly CreateDeliveryFromOrderUseCase _createDelivery;
    private readonly ILogger<OrderPaidEventConsumer> _logger;

    public OrderPaidEventConsumer(CreateDeliveryFromOrderUseCase createDelivery, ILogger<OrderPaidEventConsumer> logger)
    {
        _createDelivery = createDelivery;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderPaidEvent> context)
    {
        await _createDelivery.HandleAsync(context.Message.OrderId, context.CancellationToken);
        LogDeliveryCreated(context.Message.OrderId);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Delivery created for OrderId={OrderId}")]
    private partial void LogDeliveryCreated(Guid orderId);
}
