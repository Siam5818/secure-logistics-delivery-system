namespace DeliveryService.Domain.Entities;

public sealed class Delivery
{
    public Guid Id { get; }
    public Guid OrderId { get; }
    public DeliveryStatus Status { get; private set; }

    private Delivery() { }

    public Delivery(Guid orderId)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Status = DeliveryStatus.Pending;
    }

    public void MarkAsInTransit()
    {
        if (Status != DeliveryStatus.Pending)
            throw new InvalidOperationException($"Cannot mark as in transit a delivery with status '{Status}'.");
        Status = DeliveryStatus.InTransit;
    }

    public void MarkAsDelivered()
    {
        if (Status != DeliveryStatus.InTransit)
            throw new InvalidOperationException($"Cannot mark as delivered a delivery with status '{Status}'.");
        Status = DeliveryStatus.Delivered;
    }
}
