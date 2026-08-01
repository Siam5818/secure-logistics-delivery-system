using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.ValueObjects;

namespace OrderService.Application.UseCases;

public sealed record CreateOrderRequest(
    string Street, string City, string PostalCode, string Country, string Currency);

public sealed class CreateOrderUseCase
{
    private readonly IOrderRepository _repository;

    public CreateOrderUseCase(IOrderRepository repository) => _repository = repository;

    public async Task<Guid> HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var address = new Address(request.Street, request.City, request.PostalCode, request.Country);
        var order = new Order(address, request.Currency);

        await _repository.AddAsync(order, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
