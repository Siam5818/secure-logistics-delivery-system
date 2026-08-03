using Microsoft.EntityFrameworkCore;
using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Entities;

namespace DeliveryService.Infrastructure.Persistence;

public sealed class DeliveryRepository : IDeliveryRepository
{
    private readonly DeliveryDbContext _context;

    public DeliveryRepository(DeliveryDbContext context) => _context = context;

    public async Task<Delivery?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _context.Deliveries.FirstOrDefaultAsync(d => d.OrderId == orderId, cancellationToken);
    }

    public async Task AddAsync(Delivery delivery, CancellationToken cancellationToken = default)
    {
        await _context.Deliveries.AddAsync(delivery, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
