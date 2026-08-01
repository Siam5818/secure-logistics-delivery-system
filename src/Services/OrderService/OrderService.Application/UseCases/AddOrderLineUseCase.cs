using OrderService.Application.Interfaces;
using OrderService.Domain.ValueObjects;

namespace OrderService.Application.UseCases;

public sealed record AddOrderLineRequest(
    Guid OrderId, string ProductName, int Quantity, decimal UnitPrice, string Currency);

public sealed class AddOrderLineUseCase
{
    private readonly IOrderRepository _repository;

    public AddOrderLineUseCase(IOrderRepository repository) => _repository = repository;

    public async Task HandleAsync(AddOrderLineRequest request, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order '{request.OrderId}' not found.");

        order.AddLine(request.ProductName, request.Quantity, new Money(request.UnitPrice, request.Currency));

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
