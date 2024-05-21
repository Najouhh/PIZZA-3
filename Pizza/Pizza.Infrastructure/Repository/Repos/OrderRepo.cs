using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pizza.Infrastructure.Data;
using Pizza.Infrastructure.Repository.Interfaces;
using pizzariaV1.Data.Models.Entities;

namespace Pizza.Infrastructure.Repository.Repos
{
    public class OrderRepo : IOrderRepo
    {
        private readonly PizzaContext _context;
        private readonly IMapper _mapper;

        public OrderRepo(PizzaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);

            await _context.SaveChangesAsync();
        }
        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Dish)
                .Where(o => o.ApplicationUser.Id == userId)
                .ToListAsync();
        }
    }
}
