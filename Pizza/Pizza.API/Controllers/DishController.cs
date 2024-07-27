using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza.Application.Core.Interfaces;
using Pizza.Data.Models.DTOS;
using Pizza.Data.Models.Entities;
using System.ComponentModel;


namespace Pizza.API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
  
    public class DishController : Controller
    {
        private readonly IDishService _dishService;
        private readonly ILogger<DishController> _logger;

        public DishController(IDishService dishService, ILogger<DishController> logger)
        {
            _dishService = dishService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("Get All Dishes")]
       
        public async Task<IActionResult> GetAllDishes()
        {
            try
            {
                _logger.LogInformation("Fetching all dishes.");
                var dishes = await _dishService.GetAllDishes();
                return Ok(dishes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all dishes.");
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Get All Categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _dishService.GetAllCategories();
            return Ok(categories);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Get All Ingredients")]
        public async Task<IActionResult> GetAllIngredients()
        {
            var Ingredients = await _dishService.GetAllIngredients();
            return Ok(Ingredients);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update dish")]
        public async Task<IActionResult> UpdateDish( DishDto updatedDishDto)
        {
            try
            {
                await _dishService.UpdateDish(updatedDishDto);
                return Ok("Dish updated successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the dish.");
                return BadRequest("Something went Wrong");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add a new dish")]
        public async Task<IActionResult> AddDish(DishDto dishDto)
        {
            try
            {
                await _dishService.AddDish(dishDto);
                return Ok("Dish added successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding the dish.");
                return BadRequest("An unexpected error occurred. Please try again later.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add a new Category")]
        public async Task<IActionResult> AddCategory(Category category)
        {
            await _dishService.AddCategory(category);
            return Ok("Category added successfully!");
        }
        [Authorize(Roles = "Admin")]

        [HttpPost("add a new Ingredient")]
        public async Task<IActionResult> AddIngredients(Ingredient ingredient)
        {
            await _dishService.AddIngredients(ingredient);
            return Ok("Category added successfully!");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete A dish")]
        public async Task<IActionResult> DeleteDish(int DishID)
        {
            await _dishService.DeleteDish(DishID);
            return Ok("Dish has been deleted");
        }
    }

}
