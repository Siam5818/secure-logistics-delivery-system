using DeliveryService.Domain.Entities;

namespace DeliveryService.Application.Interfaces;

public interface IDeliveryRepository
{
    Task<Delivery?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task AddAsync(Delivery delivery, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
