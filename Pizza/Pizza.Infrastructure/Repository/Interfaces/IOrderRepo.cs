
using pizzariaV1.Data.Models.Entities;

namespace Pizza.Infrastructure.Repository.Interfaces
{
    public interface IOrderRepo
    {
        Task AddOrder(Order entity);
        Task<List<Order>> GetOrdersByUserID(string userId);
        Task<Order> GetOrderByOrderID(int OrderID);
        Task UpdateOrder(Order order);
        Task<List<Order>> GetAllOrders();
        Task DeleteOrder(int OrderID);
    }
}
