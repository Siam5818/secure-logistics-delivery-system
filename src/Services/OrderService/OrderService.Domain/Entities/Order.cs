using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Entities;

public sealed class Order
{
    private readonly List<OrderLine> _lines = new();
    private readonly string _currency;

    public Guid Id { get; }
    public OrderStatus Status { get; private set; }
    public Address ShippingAddress { get; }
    public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

    public Money Total => _lines
        .Select(l => l.LineTotal)
        .Aggregate(Money.Zero(_currency), (acc, line) => acc.Add(line));

    public Order(Address shippingAddress, string currency)
    {
        Id = Guid.NewGuid();
        ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
        _currency = currency;
        Status = OrderStatus.Created;
    }

    // Invariant 1 : impossible d'ajouter une ligne hors du statut "Created"
    public void AddLine(string productName, int quantity, Money unitPrice)
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException(
                $"Cannot add a line to an order with status '{Status}'.");

        _lines.Add(new OrderLine(productName, quantity, unitPrice));
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException($"Cannot mark as paid an order with status '{Status}'.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot pay an order with no lines.");

        Status = OrderStatus.Paid;
    }

    // Invariant 2 : impossible d'expédier une commande non payée
    public void MarkAsShipped()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException(
                $"Cannot ship an order that is not paid. Current status: '{Status}'.");

        Status = OrderStatus.Shipped;
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException(
                $"Cannot mark as delivered an order that is not shipped. Current status: '{Status}'.");

        Status = OrderStatus.Delivered;
    }

    // Invariant 3 : impossible d'annuler une commande déjà livrée/annulée
    public void Cancel()
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel an order with status '{Status}'.");

        Status = OrderStatus.Cancelled;
    }
}
