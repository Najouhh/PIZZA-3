using Pizza.Data.Models.DTOS.Order;
using pizzariaV1.Data.Models.Entities;

namespace Pizza.Application.Core.Interfaces
{
    public interface IOrderService
    {
        Task<bool> PlaceOrderAsync(OrderDto orderDto, string userId);
        Task<List<Order>> GetUserOrdersAsync(string userId);
    }
}
