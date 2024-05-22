using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza.Application.Core.Interfaces;
using Pizza.Application.Core.Services;
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
        [HttpGet("Get Order by UserID")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is required.");
            }

            var orders = await _orderService.GetUserOrders(userId);
            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found for the user.");
            }

            return Ok(orders);
        }
        [HttpPost("change Order Status")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, string newStatus)
        {
            var success = await _orderService.ChangeOrderStatus(orderId, newStatus);
            if (success)
                return Ok("Order status changed successfully.");
            else
                return NotFound("Order not found.");
        }
        [HttpGet("Get All Orders")]
        public async Task<IActionResult> GetAllOrders()
        {
           var Orders= await _orderService.GetAllOrders();
         
            return Ok(Orders);
        }
    }
}

