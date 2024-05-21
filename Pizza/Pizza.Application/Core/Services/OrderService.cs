

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Pizza.Application.Core.Interfaces;
using Pizza.Data;
using Pizza.Data.Models.DTOS.Order;
using Pizza.Data.Models.Entities;
using Pizza.Infrastructure.Repository.Interfaces;
using pizzariaV1.Data.Models.Entities;

namespace Pizza.Application.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDishRepo _dishRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderService(IDishRepo dishRepo, IOrderRepo orderRepo, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _dishRepo = dishRepo;
            _orderRepo = orderRepo;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<bool> PlaceOrderAsync(OrderDto orderDto, string userId)
        {
            // Retrieve dishes from the database based on the dish IDs provided in the order DTO
            var orderedDishes = await GetDishesFromDto(orderDto);

            if (orderedDishes.Count == 0)
            {
                // Handle case where no dishes were found
                return false;
            }

            // Calculate total price based on the ordered dishes and their quantities
            decimal totalPrice = CalculateTotalPrice(orderedDishes, orderDto.DishQuantities);

            // Apply a 20% discount if the total quantity of ordered dishes is three or more
            int totalQuantity = orderDto.DishQuantities.Sum();
            if (totalQuantity >= 3)
            {
                totalPrice *= 0.8m; // Apply 20% discount
            }

            // Create order entity from DTO
            var order = _mapper.Map<Order>(orderDto);

            // Set order date
            order.OrderDate = DateTime.Now;

            // Set order status to "Pending"
            order.OrderStatus = "Pending";

            // Assign ordered dishes to the order with their respective quantities
            order.OrderDetails = new List<OrderDetail>();
            for (int i = 0; i < orderedDishes.Count; i++)
            {
                order.OrderDetails.Add(new OrderDetail { Dish = orderedDishes[i], Quantity = orderDto.DishQuantities[i] });
            }

            // Set total price
            order.TotalPrice = totalPrice;

            // Retrieve user from the provided userId
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Handle case where user is not found
                return false;
            }

            // Check if the user is a premium user
            var roles = await _userManager.GetRolesAsync(user);
            bool isPremiumUser = roles.Contains("Premium");

            // Calculate total points earned by the user (10 points per dish)
            int totalPointsEarned = CalculateTotalPoints(orderDto.DishQuantities);

            // Award points to the user if they are a premium user
            if (isPremiumUser)
            {
                user.UserPoints += totalPointsEarned;
            }

            // Redeem points for a free pizza if the user has 100 or more points
            if (user.UserPoints >= 100)
            {
                // Assume that a free pizza is the first dish in the order
                var firstOrderDetail = order.OrderDetails.FirstOrDefault();
                if (firstOrderDetail != null)
                {
                    firstOrderDetail.Quantity += 1;
                    user.UserPoints -= 100; // Deduct 100 points for the free pizza
                }
            }

            // Set the ApplicationUser property of the order
            order.ApplicationUser = user;

            // Add order to the repository
            await _orderRepo.AddAsync(order);

            return true;
        }

        private decimal CalculateTotalPrice(List<Dish> orderedDishes, List<int> dishQuantities)
        {
            decimal totalPrice = 0;
            for (int i = 0; i < orderedDishes.Count; i++)
            {
                // Assuming each dish has a Price property
                totalPrice += (orderedDishes[i].Price ?? 0) * dishQuantities[i]; // Multiply by quantity
            }
            return totalPrice;
        }

        private int CalculateTotalPoints(List<int> dishQuantities)
        {
            // Assuming user gets 10 points for every dish they buy
            int totalPoints = dishQuantities.Sum() * 10;
            return totalPoints;
        }

        private async Task<List<Dish>> GetDishesFromDto(OrderDto orderDto)
        {
            var orderedDishes = new List<Dish>();
            foreach (var dishId in orderDto.DishIds)
            {
                var dish = await _dishRepo.GetByIdAsync(dishId);
                if (dish != null)
                {
                    orderedDishes.Add(dish);
                }
            }
            return orderedDishes;
        }
        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orderRepo.GetOrdersByUserIdAsync(userId);
        }
    }
}
