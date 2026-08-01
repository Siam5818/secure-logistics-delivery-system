using OrderService.Application.Interfaces;

namespace OrderService.Application.UseCases;

public sealed class PayOrderUseCase
{
    private readonly IOrderRepository _repository;

    public PayOrderUseCase(IOrderRepository repository) => _repository = repository;

    public async Task HandleAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order '{orderId}' not found.");

        order.MarkAsPaid();

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
