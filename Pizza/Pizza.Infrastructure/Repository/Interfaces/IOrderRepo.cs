
using pizzariaV1.Data.Models.Entities;

namespace Pizza.Infrastructure.Repository.Interfaces
{
    public interface IOrderRepo
    {
        Task AddAsync(Order entity);
        Task<List<Order>> GetOrdersByUserID(string userId);
        Task<Order> GetByIdAsync(int OrderID);
        Task UpdateAsync(Order order);
        Task<List<Order>> GetAllOrders();
    }
}
