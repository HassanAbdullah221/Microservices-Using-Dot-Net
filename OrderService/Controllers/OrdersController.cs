using Microsoft.AspNetCore.Mvc;
using OrderService.DTOs;
using OrderService.Models;
namespace OrderService.Controllers;
using OrderService.Services.Interface;
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;

    public OrdersController(IOrderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _service.GetAllAsync();
        return Ok(orders);
    }
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var orders = await _service.GetByUserIdAsync(userId);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderDto dto)
    {
        string jwtToken = string.Empty;
        if (Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            jwtToken = authHeader.ToString().Replace("Bearer ", "").Trim();
        }

        var order = new Order
        {
            UserId = dto.UserId,
            ProductName = dto.ProductName,
            Quantity = dto.Quantity
        };

        try
        {
            var createdOrder = await _service.CreateAsync(order, jwtToken);
            return Ok(createdOrder);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}