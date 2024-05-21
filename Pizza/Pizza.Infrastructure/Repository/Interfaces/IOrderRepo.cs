
using pizzariaV1.Data.Models.Entities;

namespace Pizza.Infrastructure.Repository.Interfaces
{
    public interface IOrderRepo
    {
        Task AddAsync(Order entity);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
    }
}
