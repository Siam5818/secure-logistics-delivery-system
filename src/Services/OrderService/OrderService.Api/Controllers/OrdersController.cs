using Microsoft.AspNetCore.Mvc;
using OrderService.Application.UseCases;
using Microsoft.AspNetCore.Authorization;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly CreateOrderUseCase _createOrder;
    private readonly AddOrderLineUseCase _addOrderLine;
    private readonly PayOrderUseCase _payOrder;
    private readonly GetOrderByIdUseCase _getOrderById;

    public OrdersController(
        CreateOrderUseCase createOrder,
        AddOrderLineUseCase addOrderLine,
        PayOrderUseCase payOrder,
        GetOrderByIdUseCase getOrderById)
    {
        _createOrder = createOrder;
        _addOrderLine = addOrderLine;
        _payOrder = payOrder;
        _getOrderById = getOrderById;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var orderId = await _createOrder.HandleAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = orderId }, new { id = orderId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var order = await _getOrderById.HandleAsync(id, cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost("{id:guid}/lines")]
    public async Task<IActionResult> AddLine(Guid id, AddOrderLineDto dto, CancellationToken cancellationToken)
    {
        await _addOrderLine.HandleAsync(
            new AddOrderLineRequest(id, dto.ProductName, dto.Quantity, dto.UnitPrice, dto.Currency),
            cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/pay")]
    public async Task<IActionResult> Pay(Guid id, CancellationToken cancellationToken)
    {
        await _payOrder.HandleAsync(id, cancellationToken);
        return NoContent();
    }
}

public sealed record AddOrderLineDto(string ProductName, int Quantity, decimal UnitPrice, string Currency);
