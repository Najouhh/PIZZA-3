using Pizza.Data.Models.DTOS.Order;
using pizzariaV1.Data.Models.Entities;
using System.Threading.Tasks;

namespace Pizza.Application.Core.Interfaces
{
    public interface IOrderService
    {
        Task<bool> PlaceOrderAsync(OrderDto orderDto, string userId);
        Task<List<Order>> GetUserOrders(string userId);
        Task<bool> ChangeOrderStatus(int orderId, string newStatus);
        Task<List<Order>> GetAllOrders();
        Task DeleteOrder(int OrderID);
    }
}
