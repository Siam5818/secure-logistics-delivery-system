using Microsoft.AspNetCore.Mvc;
using DeliveryService.Application.Interfaces;

namespace DeliveryService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class DeliveriesController : ControllerBase
{
    private readonly IDeliveryRepository _repository;

    public DeliveriesController(IDeliveryRepository repository) => _repository = repository;

    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetByOrderId(Guid orderId, CancellationToken cancellationToken)
    {
        var delivery = await _repository.GetByOrderIdAsync(orderId, cancellationToken);
        return delivery is null ? NotFound() : Ok(delivery);
    }
}
