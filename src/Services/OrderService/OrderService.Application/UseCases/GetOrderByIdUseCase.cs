using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.UseCases;

public sealed class GetOrderByIdUseCase
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdUseCase(IOrderRepository repository) => _repository = repository;

    public async Task<Order?> HandleAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(orderId, cancellationToken);
    }
}
