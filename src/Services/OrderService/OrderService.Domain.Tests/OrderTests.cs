using OrderService.Domain.Entities;
using OrderService.Domain.ValueObjects;
using Xunit;

namespace OrderService.Domain.Tests;

public class OrderTests
{
    private static Order CreateOrderWithOneLine()
    {
        var address = new Address("1 Rue de la Paix", "Dakar", "10000", "Senegal");
        var order = new Order(address, "XOF");
        order.AddLine("Laptop", 1, new Money(500_000, "XOF"));
        return order;
    }

    [Fact]
    public void AddLine_WhenOrderIsNotCreatedStatus_ThrowsInvalidOperationException()
    {
        var order = CreateOrderWithOneLine();
        order.MarkAsPaid();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            order.AddLine("Mouse", 1, new Money(10_000, "XOF")));

        Assert.Contains("Cannot add a line", exception.Message);
    }

    [Fact]
    public void MarkAsShipped_WhenOrderIsNotPaid_ThrowsInvalidOperationException()
    {
        var order = CreateOrderWithOneLine();

        var exception = Assert.Throws<InvalidOperationException>(() => order.MarkAsShipped());

        Assert.Contains("Cannot ship an order", exception.Message);
    }

    [Fact]
    public void Cancel_WhenOrderIsAlreadyDelivered_ThrowsInvalidOperationException()
    {
        var order = CreateOrderWithOneLine();
        order.MarkAsPaid();
        order.MarkAsShipped();
        order.MarkAsDelivered();

        var exception = Assert.Throws<InvalidOperationException>(() => order.Cancel());

        Assert.Contains("Cannot cancel an order", exception.Message);
    }

    [Fact]
    public void Money_Constructor_WhenAmountIsNegative_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Money(-10, "XOF"));
    }

    [Fact]
    public void Total_WhenOrderHasMultipleLines_SumsCorrectly()
    {
        var address = new Address("1 Rue de la Paix", "Dakar", "10000", "Senegal");
        var order = new Order(address, "XOF");
        order.AddLine("Laptop", 1, new Money(500_000, "XOF"));
        order.AddLine("Mouse", 2, new Money(10_000, "XOF"));

        Assert.Equal(520_000, order.Total.Amount);
    }
}
