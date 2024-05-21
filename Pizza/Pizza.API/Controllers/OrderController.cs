using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza.Application.Core.Interfaces;
using Pizza.Data.Models.DTOS.Order;
using System.Security.Claims;

namespace Pizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(OrderDto orderDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is required.");
            }

            if (orderDto == null || orderDto.DishIds == null || orderDto.DishIds.Count == 0)
            {
                return BadRequest("Invalid order data.");
            }

            bool orderPlaced = await _orderService.PlaceOrderAsync(orderDto, userId);
            if (orderPlaced)
            {
                return Ok("Order placed successfully.");
            }
            else
            {
                return BadRequest("Failed to place order.");
            }
        }
        [HttpGet("user-orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is required.");
            }

            var orders = await _orderService.GetUserOrdersAsync(userId);
            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found for the user.");
            }

            return Ok(orders);
        }
    }
}

