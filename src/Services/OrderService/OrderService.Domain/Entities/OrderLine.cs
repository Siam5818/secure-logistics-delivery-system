using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Entities;

public sealed class OrderLine
{
    public Guid Id { get; }
    public string ProductName { get; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; }

    internal OrderLine(string productName, int quantity, Money unitPrice)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required.", nameof(productName));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        Id = Guid.NewGuid();
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Money LineTotal => UnitPrice.Multiply(Quantity);
}
