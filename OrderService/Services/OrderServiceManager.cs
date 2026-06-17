using System.Net;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using OrderService.Services.Interface;

namespace OrderService.Services;

public class OrderServiceManager : IOrderService
{
    private readonly OrderDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public OrderServiceManager(OrderDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<List<Order>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }
    public async Task<Order> CreateAsync(Order order, string jwtToken)
    {
        var client = _httpClientFactory.CreateClient("UserClient");

        if (!string.IsNullOrEmpty(jwtToken))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwtToken);
        }

        string requestUrl = $"api/users/{order.UserId}";

        HttpResponseMessage response;

        try
        {
            response = await client.GetAsync(requestUrl);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception(
                "Unable to contact the User Microservice. Please check network settings.",
                ex
            );
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new InvalidOperationException(
                $"Validation Failed: User with ID {order.UserId} does not exist."
            );
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized ||
            response.StatusCode == HttpStatusCode.Forbidden)
        {
            throw new UnauthorizedAccessException(
                "Validation Failed: Unauthorized access to User Microservice."
            );
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Validation Failed: User Microservice responded with {response.StatusCode}"
            );
        }

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return order;
    }
}