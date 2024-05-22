﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pizza.Infrastructure.Data;
using Pizza.Infrastructure.Migrations;
using Pizza.Infrastructure.Repository.Interfaces;
using pizzariaV1.Data.Models.Entities;

namespace Pizza.Infrastructure.Repository.Repos
{
    public class OrderRepo : IOrderRepo
    {
        private readonly PizzaContext _context;

        public OrderRepo(PizzaContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);

            await _context.SaveChangesAsync();
        }
        public async Task<Order> GerOderByID(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders
           .Include(d => d.OrderDetails)
           .ToListAsync();
        }
        public async Task<List<Order>> GetOrdersByUserID(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Dish)
                .Where(o => o.ApplicationUser.Id == userId)
                .ToListAsync();
        }
        public async Task<Order> GetByIdAsync(int OrderID)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == OrderID);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
