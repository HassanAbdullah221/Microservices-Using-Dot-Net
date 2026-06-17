namespace OrderService.Services.Interface;

using OrderService.Models;
public interface IOrderService
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetByUserIdAsync(Guid userId);
    Task<Order> CreateAsync(Order order, string jwtToken);
}