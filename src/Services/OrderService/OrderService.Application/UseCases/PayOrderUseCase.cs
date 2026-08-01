using MassTransit;
using OrderService.Application.Events;
using OrderService.Application.Interfaces;

namespace OrderService.Application.UseCases;

public sealed class PayOrderUseCase
{
    private readonly IOrderRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public PayOrderUseCase(IOrderRepository repository, IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task HandleAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order '{orderId}' not found.");

        order.MarkAsPaid();
        await _repository.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(
            new OrderPaidEvent(order.Id, order.Total.Amount, order.Total.Currency, DateTime.UtcNow),
            cancellationToken);
    }
}
