using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Entities;

namespace DeliveryService.Infrastructure.Persistence;

public sealed class DeliveryDbContext : DbContext
{
    public DbSet<Delivery> Deliveries => Set<Delivery>();

    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Delivery>(builder =>
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedNever();
            builder.Property(d => d.OrderId);
            builder.Property(d => d.Status);
        });
    }
}
