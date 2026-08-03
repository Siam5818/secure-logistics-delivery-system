using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Entities;

namespace DeliveryService.Application.UseCases;

public sealed class CreateDeliveryFromOrderUseCase
{
    private readonly IDeliveryRepository _repository;

    public CreateDeliveryFromOrderUseCase(IDeliveryRepository repository) => _repository = repository;

    public async Task HandleAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByOrderIdAsync(orderId, cancellationToken);
        if (existing is not null) return; // idempotence : évite un doublon si l'événement est livré deux fois

        var delivery = new Delivery(orderId);
        await _repository.AddAsync(delivery, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
