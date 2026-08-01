using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence;

public sealed class OrderDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(builder =>
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();

            builder.OwnsOne(o => o.ShippingAddress, address =>
            {
                address.Property(a => a.Street);
                address.Property(a => a.City);
                address.Property(a => a.PostalCode);
                address.Property(a => a.Country);
            });

            builder.Ignore(o => o.Total);

            builder.Property<string>("_currency").HasColumnName("Currency");

            builder.OwnsMany(o => o.Lines, lineBuilder =>
            {
                lineBuilder.WithOwner().HasForeignKey("OrderId");
                lineBuilder.HasKey(l => l.Id);
                lineBuilder.Property(l => l.Id).ValueGeneratedNever();
                lineBuilder.Property(l => l.ProductName);
                lineBuilder.Property(l => l.Quantity);
                lineBuilder.OwnsOne(l => l.UnitPrice, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("UnitPriceAmount");
                    money.Property(m => m.Currency).HasColumnName("UnitPriceCurrency");
                });
            });

            // Uniquement ICI : Lines a besoin de l'accès par champ,
            // car la propriété publique recrée un wrapper à chaque lecture.
            builder.Navigation(o => o.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }
}
